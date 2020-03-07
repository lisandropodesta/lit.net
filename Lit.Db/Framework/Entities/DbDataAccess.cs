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

        protected DbDataAccess(string connectionString) : base(connectionString) { }

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
        /// Gets a record by code.
        /// </summary>
        public T GetBytCode<T>(string code)
            where T : DbTableTemplate, new()
        {
            var record = new T();
            record.SetCode(code);
            GetByCode(record);
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
        /// Gets a record by id.
        /// </summary>
        public void Get<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Get);
        }

        /// <summary>
        /// Gets a record by code.
        /// </summary>
        public void GetByCode<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.GetByCode);
        }

        /// <summary>
        /// Inserts a record.
        /// </summary>
        public void Insert<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Insert);
        }

        /// <summary>
        /// Inserts and reads a record.
        /// </summary>
        public void InsertGet<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.InsertGet);
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        public void Update<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Update);
        }

        /// <summary>
        /// Updates and reads a record.
        /// </summary>
        public void UpdateGet<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.UpdateGet);
        }

        /// <summary>
        /// Inserts or updates a record.
        /// </summary>
        public void Set<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.Set);
        }

        /// <summary>
        /// Inserts or updates and reads a record.
        /// </summary>
        public void SetGet<T>(T record)
        {
            ExecuteTableSp(record, StoredProcedureFunction.SetGet);
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
            var spName = GetStoredProcedureName(typeof(T), StoredProcedureFunction.ListAll);
            ExecuteTemplate(template, spName, CommandType.StoredProcedure);
            return template.Result;
        }

        /// <summary>
        /// Executes a table stored procedure over a single record.
        /// </summary>
        protected void ExecuteTableSp(object record, StoredProcedureFunction spFunc)
        {
            var cmdType = (CommandType)((int)CommandType.TableDirect + spFunc);
            ExecuteTemplate(record, null, cmdType);
        }

        /// <summary>
        /// Get standard stored procedure name.
        /// </summary>
        protected string GetStoredProcedureName(Type tableTemplate, StoredProcedureFunction spFunc)
        {
            var binding = DbTemplateCache.GetTable(tableTemplate, DbNaming);
            return DbNaming.GetStoredProcedureName(binding.Text, spFunc);
        }

        /// <summary>
        /// Table stored procedure resolving.
        /// </summary>
        protected override string GetTableSpName(DbTemplateBinding binding, CommandType cmdType)
        {
            var spFunc = (StoredProcedureFunction)(cmdType - CommandType.TableDirect);
            return DbNaming.GetStoredProcedureName(binding.Text, spFunc);
        }

        /// <summary>
        /// Set table stored procedure parameters.
        /// </summary>
        protected override void SetTableSpInputParameters<T>(DbTemplateBinding binding, DbCommand cmd, T instance, CommandType cmdType)
        {
            var spFunc = (StoredProcedureFunction)(cmdType - CommandType.TableDirect);
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

                case StoredProcedureFunction.GetByCode:
                    return DbColumnsSelection.UniqueKey;

                case StoredProcedureFunction.ListAll:
                    return DbColumnsSelection.None;

                case StoredProcedureFunction.Insert:
                case StoredProcedureFunction.InsertGet:
                    return DbColumnsSelection.NonPrimaryKey;

                case StoredProcedureFunction.Update:
                case StoredProcedureFunction.UpdateGet:
                case StoredProcedureFunction.Set:
                case StoredProcedureFunction.SetGet:
                    return DbColumnsSelection.All;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
