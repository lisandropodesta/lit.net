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
        /// Executes a query template.
        /// </summary>
        public T ExecuteQuery<T>(string query, Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(query, DbTemplateKind.Query, setup);
        }

        /// <summary>
        /// Executes a query template.
        /// </summary>
        public T ExecuteQuery<T>(Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(null, DbTemplateKind.Query, setup);
        }

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        public T ExecuteStoredProcedure<T>(string storedProcedureName, Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(storedProcedureName, DbTemplateKind.StoredProcedure, setup);
        }

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        public void ExecuteStoredProcedure<T>(string storedProcedureName, T template)
        {
            ExecuteTemplate(template, storedProcedureName, DbTemplateKind.StoredProcedure, null);
        }

        /// <summary>
        /// Execute a stored procedure/query template.
        /// </summary>
        public T ExecuteTemplate<T>(Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(null, null, setup);
        }

        /// <summary>
        /// Execute a stored procedure/query template.
        /// </summary>
        public void ExecuteTemplate<T>(T template)
        {
            ExecuteTemplate(template, null, null, null);
        }

        /// <summary>
        /// Initializes and executes a stored procedure or query template.
        /// </summary>
        private T ExecuteTemplate<T>(string text, DbTemplateKind? kind, Action<T> setup)
            where T : new()
        {
            var template = new T();
            ExecuteTemplate(template, text, kind, setup);
            return template;
        }

        /// <summary>
        /// Execute a stored procedure or query with a template already initialized.
        /// </summary>
        private void ExecuteTemplate<T>(T template, string text, DbTemplateKind? kind, Action<T> setup)
        {
            setup?.Invoke(template);

            var binding = DbTemplateBinding<TS>.Get(typeof(T), DbNaming);

            var isStoredProcedure = (kind ?? binding.Kind) == DbTemplateKind.StoredProcedure;

            if (string.IsNullOrEmpty(text))
            {
                text = binding.Text;
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentException($"Class {typeof(T).FullName} has no stored procedure / query defined");
                }
            }

            if (!isStoredProcedure)
            {
                text = binding.SetInputParameters(text, template);
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
