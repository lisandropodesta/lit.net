using System;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db record property binding.
    /// </summary>
    internal class DbRecordBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordAttribute>, IDbRecordBinding
        where TC : class
    {
        #region Constructor

        public DbRecordBinding(IDbSetup setup, PropertyInfo propInfo, DbRecordAttribute attr)
            : base(setup, propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(DbDataReader reader, object instance)
        {
            var list = DbHelper.LoadSqlRecordset(reader, BindingType, Attributes.AllowMultipleRecords ? 1 : 2, Setup) as IList;

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

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected override object DecodePropertyValue(TP value)
        {
            return value;
        }

        /// <summary>
        /// Value encoding to property type.
        /// </summary>
        protected override TP EncodePropertyValue(object value)
        {
            return value != null ? (TP)value : default;
        }
    }
}
