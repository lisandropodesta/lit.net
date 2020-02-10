using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Lit.Db.Interface;

namespace Lit.Db.Class
{
    /// <summary>
    /// Db commands implementation.
    /// </summary>
    public abstract class DbCommands : IDbCommands
    {
        // Stored procedures cache
        private static readonly Dictionary<string, DbStoredProcedure> storedProcedures = new Dictionary<string, DbStoredProcedure>();

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
            var binding = DbTemplateBinding.Get(typeof(T));

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                storedProcedureName = binding.StoredProcedureName;
                if (string.IsNullOrEmpty(storedProcedureName))
                {
                    throw new ArgumentException($"Class {typeof(T).Name} has no stored procedure name defined (attribute DbStoredProcedureAttribute)");
                }
            }

            using (var connection = GetSqlConnection())
            {
                using (var cmd = GetSqlCommand(storedProcedureName, connection))
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
                                binding.LoadResults(reader, template);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a sql command attached to the current transaction and ready to be executed.
        /// </summary>
        private SqlCommand GetSqlCommand(string name, SqlConnection connection)
        {
            var sp = GetStoredProcedure(name, connection);
            return sp.GetSqlCommand(connection);
        }

        /// <summary>
        /// Gets a stored procedure information.
        /// </summary>
        private DbStoredProcedure GetStoredProcedure(string name, SqlConnection connection)
        {
            lock (storedProcedures)
            {
                if (!storedProcedures.TryGetValue(name, out var sp))
                {
                    sp = new DbStoredProcedure(name, connection);
                    storedProcedures.Add(name, sp);
                }

                return sp;
            }
        }

        protected abstract SqlConnection GetSqlConnection();
    }
}
