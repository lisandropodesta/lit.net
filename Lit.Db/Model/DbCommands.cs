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
        /// Db setup.
        /// </summary>
        public IDbSetup Setup { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DbCommands(IDbSetup setup)
        {
            Setup = setup;
        }

        /// <summary>
        /// Executes a query template.
        /// </summary>
        public T ExecuteQuery<T>(string query, Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(query, CommandType.Text, setup);
        }

        /// <summary>
        /// Executes a query template.
        /// </summary>
        public T ExecuteQuery<T>(Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(null, CommandType.Text, setup);
        }

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        public T ExecuteStoredProcedure<T>(string storedProcedureName, Action<T> setup = null)
            where T : new()
        {
            return ExecuteTemplate(storedProcedureName, CommandType.StoredProcedure, setup);
        }

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        public void ExecuteStoredProcedure<T>(string storedProcedureName, T template)
        {
            ExecuteTemplate(template, storedProcedureName, CommandType.StoredProcedure);
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
        public void ExecuteTemplate(object template)
        {
            ExecuteTemplate(template, null, null);
        }

        /// <summary>
        /// Initializes and executes a stored procedure or query template.
        /// </summary>
        protected T ExecuteTemplate<T>(string text, CommandType? commandType, Action<T> setup)
            where T : new()
        {
            var template = new T();
            setup?.Invoke(template);
            ExecuteTemplate(template, text, commandType);
            return template;
        }

        /// <summary>
        /// Execute a stored procedure or query with a template already initialized.
        /// </summary>
        protected void ExecuteTemplate<T>(T template, string text, CommandType? commandType)
        {
            var type = template?.GetType() ?? typeof(T);
            var binding = Setup.GetTemplateBinding(type);
            var cmdType = commandType ?? binding.CommandType;
            var spCmdType = cmdType;

            if (string.IsNullOrEmpty(text))
            {
                text = binding.Text;
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentException($"Class {type.FullName} has no stored procedure / query defined");
                }
            }

            if (cmdType == CommandType.Text)
            {
                text = binding.SetInputParameters(text, template);
            }
            else if (cmdType > CommandType.TableDirect)
            {
                text = GetTableSpName(binding, cmdType);
                spCmdType = CommandType.StoredProcedure;
            }

            using (var connection = GetOpenedConnection())
            {
                using (var cmd = GetCommand(text, connection, spCmdType))
                {
                    if (cmdType == CommandType.StoredProcedure)
                    {
                        binding.SetInputParameters(cmd, template);
                    }
                    else if (cmdType > CommandType.TableDirect)
                    {
                        SetTableSpInputParameters(binding, cmd, template, cmdType);
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
                                binding.LoadResults(reader, template);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a command attached to the current transaction and ready to be executed.
        /// </summary>
        protected TS GetCommand(string name, TH connection, CommandType commandType)
        {
            var cmd = CreateCommand(name, connection);
            cmd.CommandType = commandType;

            if (commandType == CommandType.StoredProcedure)
            {
                AddStoredProcedureParameters(name, cmd);
            }

            return cmd;
        }

        /// <summary>
        /// Adds the stored procedure parameters.
        /// </summary>
        protected DbStoredProcedure<TS> AddStoredProcedureParameters(string name, TS command)
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
        protected TH GetOpenedConnection()
        {
            var conn = CreateConnection();
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Table stored procedure resolving.
        /// </summary>
        protected virtual string GetTableSpName(DbTemplateBinding binding, CommandType cmdType)
        {
            throw new ArgumentException("Unable to run table query in DbHost.");
        }

        /// <summary>
        /// Set table stored procedure parameters.
        /// </summary>
        protected virtual void SetTableSpInputParameters<T>(DbTemplateBinding binding, DbCommand cmd, T template, CommandType cmdType)
        {
            throw new ArgumentException("Unable to run table query in DbHost.");
        }

        protected abstract TH CreateConnection();

        protected abstract TS CreateCommand(string name, TH connection);

        protected abstract DbStoredProcedure<TS> CreateStoredProcedure(string name, TS command);
    }
}
