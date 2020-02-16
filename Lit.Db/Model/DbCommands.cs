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
        private static readonly Dictionary<string, DbStoredProcedure<TS>> storedProcedures = new Dictionary<string, DbStoredProcedure<TS>>();

        /// <summary>
        /// Db naming manager
        /// </summary>
        public IDbNaming DbNaming;

        /// <summary>
        /// Executes a query loading results in a template.
        /// </summary>
        public T ExecuteQuery<T>(string query)
            where T : new()
        {
            var template = new T();
            ExecuteTemplate(query, template, false);
            return template;
        }

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
            ExecuteTemplate(template, storedProcedureName);
            return template;
        }

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        public void ExecuteTemplate<T>(T template)
        {
            ExecuteTemplate(template, null);
        }

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        public void ExecuteTemplate<T>(T template, string storedProcedureName)
        {
            ExecuteTemplate<T>(storedProcedureName, template, true);
        }

        /// <summary>
        /// Execute a stored procedure or query with a template already initialized.
        /// </summary>
        private void ExecuteTemplate<T>(string text, T template, bool isStoredProcedure)
        {
            var binding = DbTemplateBinding<TS>.Get(typeof(T), DbNaming);

            if (isStoredProcedure)
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = binding.StoredProcedureName;
                    if (string.IsNullOrEmpty(text))
                    {
                        throw new ArgumentException($"Class {typeof(T).Name} has no stored procedure name defined (attribute DbStoredProcedureAttribute)");
                    }
                }
            }

            using (var connection = GetConnectionOpened())
            {
                using (var cmd = GetCommand(text, connection, isStoredProcedure))
                {
                    if (isStoredProcedure)
                    {
                        binding.SetInputParameters(cmd, template);
                    }

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
        private TS GetCommand(string name, TH connection, bool isStoredProcedure)
        {
            var cmd = CreateCommand(name, connection);

            if (isStoredProcedure)
            {
                AddStoredProcedureParameters(name, cmd);
            }

            return cmd;
        }

        /// <summary>
        /// Adds the stored procedure parameters.
        /// </summary>
        private DbStoredProcedure<TS> AddStoredProcedureParameters(string name, TS command)
        {
            lock (storedProcedures)
            {
                if (!storedProcedures.TryGetValue(name, out var sp))
                {
                    sp = CreateStoredProcedure(name, command);
                    storedProcedures.Add(name, sp);
                }
                else
                {
                    sp.AddParameters(command);
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
            var conn = CreateConnection();
            conn.Open();
            return conn;
        }

        protected abstract TH CreateConnection();

        protected abstract TS CreateCommand(string name, TH connection);

        protected abstract DbStoredProcedure<TS> CreateStoredProcedure(string name, TS command);
    }
}
