using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// Standardized data access.
    /// </summary>
    public abstract class DbDataAccess<TH, TS> : DbHost<TH, TS>, IDbDataAccess
        where TH : DbConnection
        where TS : DbCommand
    {
        #region Constructors

        protected DbDataAccess(IDbSetup setup, string connectionString) : base(setup, connectionString) { }

        #endregion

        /// <summary>
        /// Gets a record by id.
        /// </summary>
        public T Get<T, TID>(TID id)
            where T : new()
        {
            var record = new T();
            SetId(record, id);
            return Get(record) ? record : default;
        }

        /// <summary>
        /// Finds a record by code.
        /// </summary>
        public T Find<T>(string code)
            where T : new()
        {
            var record = new T();
            SetCode(record, code);
            return Find(record) ? record : default;
        }

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        public void Delete<T, TID>(TID id)
            where T : new()
        {
            var record = new T();
            SetId(record, id);
            Delete(record);
        }

        /// <summary>
        /// Gets a record by primary key.
        /// </summary>
        public bool Get<T>(T record)
        {
            return ExecuteTableSp(record, StoredProcedureFunction.Get);
        }

        /// <summary>
        /// Finds a record by unique key.
        /// </summary>
        public bool Find<T>(T record)
        {
            return ExecuteTableSp(record, StoredProcedureFunction.Find);
        }

        /// <summary>
        /// Inserts a record.
        /// </summary>
        public void Insert<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Insert);
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        public void Update<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Update);
        }

        /// <summary>
        /// Single record store.
        /// </summary>
        public void Store<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Store);
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        public void Delete<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Delete);
        }

        /// <summary>
        /// List all records.
        /// </summary>
        public List<T> List<T>()
        {
            var template = new DbRecordsListSp<T>();
            var spName = GetTableSpName(typeof(T), StoredProcedureFunction.ListAll);
            ExecuteTemplate(template, spName, CommandType.StoredProcedure);
            return template.Result;
        }

        /// <summary>
        /// Gets the record id.
        /// </summary>
        public TID GetId<T, TID>(T record)
        {
            if (record is IDbId<TID> intf)
            {
                return intf.Id;
            }

            var binding = Setup.GetTableBinding(typeof(T));
            if (binding.SingleColumnPrimaryKey != null)
            {
                var value = binding.SingleColumnPrimaryKey.GetValue(record);
                return (TID)value;
            }

            throw new TemplateDoNotImplementInterface(typeof(T), typeof(IDbId<TID>));
        }

        /// <summary>
        /// Gets the record code.
        /// </summary>
        public string GetCode<T>(T record)
        {
            return Cast<T, IDbStringCode>(record).Code;
        }

        /// <summary>
        /// Sets the record id.
        /// </summary>
        public void SetId<T, TID>(T record, TID id)
        {
            if (record is IDbId<TID> intf)
            {
                intf.Id = id;
                return;
            }

            var binding = Setup.GetTableBinding(typeof(T));
            if (binding.SingleColumnPrimaryKey != null)
            {
                binding.SingleColumnPrimaryKey.SetValue(record, id);
                return;
            }

            throw new TemplateDoNotImplementInterface(typeof(T), typeof(IDbId<TID>));
        }

        /// <summary>
        /// Sets the record code.
        /// </summary>
        public void SetCode<T>(T record, string code)
        {
            Cast<T, IDbStringCode>(record).Code = code;
        }

        /// <summary>
        /// Get as interface from a class.
        /// </summary>
        private TI Cast<TR, TI>(TR record)
        {
            if (record is TI intf)
            {
                return intf;
            }

            throw new ArgumentException($"Table template [{typeof(TR).FullName}] do not implements [{typeof(TI).Name}]");
        }

        /// <summary>
        /// Executes a table stored procedure over a single record.
        /// </summary>
        protected bool ExecuteTableSp(object record, StoredProcedureFunction spFunc)
        {
            var type = record.GetType();
            var binding = Setup.GetTableBinding(type);
            var spName = binding.GetTableSpName(spFunc);

            using (var connection = GetOpenedConnection())
            {
                using (var cmd = GetCommand(spName, connection, CommandType.StoredProcedure))
                {
                    binding.SetTableSpInputParameters(cmd, record, spFunc);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        return binding.LoadResults(this, reader, record);
                    }
                }
            }
        }

        /// <summary>
        /// Get standard stored procedure name.
        /// </summary>
        protected string GetTableSpName(Type tableTemplate, StoredProcedureFunction spFunc)
        {
            var binding = Setup.GetTableBinding(tableTemplate);
            return binding.GetTableSpName(spFunc);
        }

        protected override void LoadResults<T>(IDbCommandBinding binding, DbDataReader reader, T template)
        {
            binding.LoadResults(this, reader, template);
        }

        #region Exceptions

        public class TemplateDoNotImplementInterface : ArgumentException
        {
            public TemplateDoNotImplementInterface(Type templateType, Type interfaceType)
                : base($"Table template [{templateType.FullName}] do not implements [{interfaceType.Name}]")

            {
            }
        }

        #endregion
    }
}
