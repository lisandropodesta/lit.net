using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Lit.Db.Model;

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
        public T Get<T>(int id)
            where T : DbTableTemplate, new()
        {
            var record = new T();
            record.SetId(id);
            Get(record);
            return record;
        }

        /// <summary>
        /// Gets a record by id.
        /// </summary>
        public T Get<T>(long id)
            where T : DbTableTemplate, new()
        {
            var record = new T();
            record.SetId(id);
            Get(record);
            return record;
        }

        /// <summary>
        /// Finds a record by code.
        /// </summary>
        public T Find<T>(string code)
            where T : DbTableTemplate, new()
        {
            var record = new T();
            record.SetCode(code);
            Find(record);
            return record;
        }

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        public void Delete<T>(int id)
            where T : DbTableTemplate, new()
        {
            var record = new T();
            record.SetId(id);
            Delete(record);
        }

        /// <summary>
        /// Gets a record by primary key.
        /// </summary>
        public void Get<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Get);
        }

        /// <summary>
        /// Finds a record by unique key.
        /// </summary>
        public void Find<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Find);
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
        /// Executes a table stored procedure over a single record.
        /// </summary>
        protected void ExecuteTableSp(object record, StoredProcedureFunction spFunc)
        {
            var type = record.GetType();
            var binding = Setup.GetTableBinding(type);
            var spName = GetTableSpName(binding, spFunc);

            using (var connection = GetOpenedConnection())
            {
                using (var cmd = GetCommand(spName, connection, CommandType.StoredProcedure))
                {
                    SetTableSpInputParameters(binding, cmd, record, spFunc);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        binding.LoadResults(reader, record);
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
            return GetTableSpName(binding, spFunc);
        }

        /// <summary>
        /// Table stored procedure resolving.
        /// </summary>
        protected string GetTableSpName(IDbTableBinding binding, StoredProcedureFunction spFunc)
        {
            return Setup.Naming.GetStoredProcedureName(binding.TableName, spFunc);
        }

        /// <summary>
        /// Set table stored procedure parameters.
        /// </summary>
        protected void SetTableSpInputParameters<T>(IDbTableBinding binding, DbCommand cmd, T instance, StoredProcedureFunction spFunc)
        {
            var columns = GetTableSpParameters(spFunc);
            binding.MapColumns(columns, c => c.SetInputParameters(cmd, instance));
        }

        /// <summary>
        /// Get parameters selection for a table stored procedure.
        /// </summary>
        protected DbColumnsSelection GetTableSpParameters(StoredProcedureFunction spFunc)
        {
            switch (spFunc)
            {
                case StoredProcedureFunction.Get:
                case StoredProcedureFunction.Delete:
                    return DbColumnsSelection.PrimaryKey;

                case StoredProcedureFunction.Find:
                    return DbColumnsSelection.UniqueKey;

                case StoredProcedureFunction.ListAll:
                    return DbColumnsSelection.None;

                case StoredProcedureFunction.Insert:
                    return DbColumnsSelection.NonPrimaryKey;

                case StoredProcedureFunction.Update:
                case StoredProcedureFunction.Store:
                    return DbColumnsSelection.All;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
