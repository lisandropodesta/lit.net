﻿using System;
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
        public T Get<T>(short id)
            where T : new()
        {
            var record = new T();
            SetId16(record, id);
            Get(record);
            return record;
        }

        /// <summary>
        /// Gets a record by id.
        /// </summary>
        public T Get<T>(int id)
            where T : new()
        {
            var record = new T();
            SetId32(record, id);
            Get(record);
            return record;
        }

        /// <summary>
        /// Gets a record by id.
        /// </summary>
        public T Get<T>(long id)
            where T : new()
        {
            var record = new T();
            SetId64(record, id);
            Get(record);
            return record;
        }

        /// <summary>
        /// Finds a record by code.
        /// </summary>
        public T Find<T>(string code)
            where T : new()
        {
            var record = new T();
            SetCode(record, code);
            Find(record);
            return record;
        }

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        public void Delete<T>(short id)
            where T : new()
        {
            var record = new T();
            SetId16(record, id);
            Delete(record);
        }

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        public void Delete<T>(int id)
            where T : new()
        {
            var record = new T();
            SetId32(record, id);
            Delete(record);
        }

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        public void Delete<T>(long id)
            where T : new()
        {
            var record = new T();
            SetId64(record, id);
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
        /// Sets the record id.
        /// </summary>
        public short GetId16<T>(T record)
        {
            return Cast<T, IDbId16>(record).Id;
        }

        /// <summary>
        /// Sets the record id.
        /// </summary>
        public int GetId32<T>(T record)
        {
            return Cast<T, IDbId32>(record).Id;
        }

        /// <summary>
        /// Get the record id.
        /// </summary>
        public long GetId64<T>(T record)
        {
            return Cast<T, IDbId64>(record).Id;
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
        public void SetId16<T>(T record, short id)
        {
            Cast<T, IDbId16>(record).Id = id;
        }

        /// <summary>
        /// Sets the record id.
        /// </summary>
        public void SetId32<T>(T record, int id)
        {
            Cast<T, IDbId32>(record).Id = id;
        }

        /// <summary>
        /// Sets the record id.
        /// </summary>
        public void SetId64<T>(T record, long id)
        {
            Cast<T, IDbId64>(record).Id = id;
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
