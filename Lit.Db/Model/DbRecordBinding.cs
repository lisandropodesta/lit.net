using System;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db record binding interface.
    /// </summary>
    internal interface IDbRecordBinding<TS> : IDbPropertyBinding<DbRecordAttribute>
        where TS : DbCommand
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance, IDbNaming dbNaming);
    }

    /// <summary>
    /// Db record property binding.
    /// </summary>
    internal class DbRecordBinding<TS, TC, TP> : DbPropertyBinding<TC, TP, DbRecordAttribute>, IDbRecordBinding<TS>
        where TS : DbCommand
        where TC : class
    {
        #region Constructor

        public DbRecordBinding(PropertyInfo propInfo, DbRecordAttribute attr, IDbNaming dbNaming)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(DbDataReader reader, object instance, IDbNaming dbNaming)
        {
            var list = DbHelper.LoadSqlRecordset<TS>(reader, BindingType, Attributes.AllowMultipleRecords ? 1 : 2, dbNaming) as IList;

            switch (list.Count)
            {
                case 0:
                    SetValue(instance, null);
                    break;

                case 1:
                    SetValue(instance, list[0]);
                    break;

                default:
                    throw new ArgumentException($"Stored procedure returned more than one record and a single one was expected, at index {Attributes.Index}");
            }
        }
    }
}
