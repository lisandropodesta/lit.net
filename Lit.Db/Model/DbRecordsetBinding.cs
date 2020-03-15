using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db recordset property binding.
    /// </summary>
    internal class DbRecordsetBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordsetAttribute>, IDbRecordsetBinding
        where TC : class
    {
        #region Constructor

        public DbRecordsetBinding(IDbSetup setup, PropertyInfo propInfo, DbRecordsetAttribute attr)
            : base(setup, propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(DbDataReader reader, object instance)
        {
            var list = DbHelper.LoadSqlRecordset(reader, BindingType, int.MaxValue, Setup);
            SetValue(instance, list);
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
