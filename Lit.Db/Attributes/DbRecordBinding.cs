using System;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Class
{
    /// <summary>
    /// Db record binding interface.
    /// </summary>
    public interface IDbRecordBinding : IDbPropertyBinding<DbRecordAttribute>
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(SqlDataReader reader, object instance);
    }

    /// <summary>
    /// Db record property binding.
    /// </summary>
    public class DbRecordBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordAttribute>, IDbRecordBinding where TC : class
    {
        #region Constructor

        public DbRecordBinding(PropertyInfo propInfo, DbRecordAttribute attr)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(SqlDataReader reader, object instance)
        {
            var list = DbHelper.LoadSqlRecordset(reader, BindingType, Attributes.AllowMultipleRecords ? 1 : 2) as IList;

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
