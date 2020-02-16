using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db commands implementation.
    /// </summary>
    public abstract class DbCommands<TH, TS> : IDbCommands
        where TH : DbConnection
        where TS : DbCommand
    {
        // Stored procedures cache
        private static readonly Dictionary<string, DbStoredProcedure<TH, TS>> storedProcedures = new Dictionary<string, DbStoredProcedure<TH, TS>>();

        // Db naming manager
        public IDbNaming DbNaming;

        /// <summary>
        /// Execute a stored procedure template with a parameters initialization action.
        /// </summary>
        public T ExecuteTemplate<T>(Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(null, setup);
        }

        /// <summary>
        /// Execute a stored procedure template with a parameters initialization action.
        /// </summary>
        public T ExecuteTemplate<T>(string storedProcedureName, Action<T> setup = null)
            where T : new()
        {
            var template = new T();

            setup?.Invoke(template);

            ExecuteTemplate(storedProcedureName, template);

            return template;
        }

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        public void ExecuteTemplate<T>(T template)
        {
            ExecuteTemplate(null, template);
        }

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        public void ExecuteTemplate<T>(string storedProcedureName, T template)
        {
            var binding = DbTemplateBinding<TS>.Get(typeof(T), DbNaming);

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                storedProcedureName = binding.StoredProcedureName;
                if (string.IsNullOrEmpty(storedProcedureName))
                {
                    throw new ArgumentException($"Class {typeof(T).Name} has no stored procedure name defined (attribute DbStoredProcedureAttribute)");
                }
            }

            using (var connection = GetConnectionOpened())
            {
                using (var cmd = GetCommand(storedProcedureName, connection))
                {
                    binding.SetInputParameters(cmd, template);

                    switch (binding.Mode)
                    {
                        case DbExecutionMode.NonQuery:
                            cmd.ExecuteNonQuery();
                            binding.GetOutputParameters(cmd, template);
                            break;

                        case DbExecutionMode.Query:
                            using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                binding.LoadResults(reader, template, DbNaming);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a command attached to the current transaction and ready to be executed.
        /// </summary>
        private TS GetCommand(string name, TH connection)
        {
            var sp = GetStoredProcedure(name, connection);
            return sp.GetCommand(connection);
        }

        /// <summary>
        /// Gets a stored procedure information.
        /// </summary>
        private DbStoredProcedure<TH, TS> GetStoredProcedure(string name, TH connection)
        {
            lock (storedProcedures)
            {
                if (!storedProcedures.TryGetValue(name, out var sp))
                {
                    sp = CreateStoredProcedure(name, connection);
                    storedProcedures.Add(name, sp);
                }

                return sp;
            }
        }

        /// <summary>
        /// Gets an opened connection.
        /// </summary>
        /// <returns></returns>
        protected TH GetConnectionOpened()
        {
            var conn = GetConnection();
            conn.Open();
            return conn;
        }

        protected abstract DbStoredProcedure<TH, TS> CreateStoredProcedure(string name, TH connection);

        protected abstract TH GetConnection();
    }
}
