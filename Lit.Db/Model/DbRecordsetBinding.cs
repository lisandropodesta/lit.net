using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db recordset binding interface.
    /// </summary>
    public interface IDbRecordsetBinding : IDbPropertyBinding<DbRecordsetAttribute>
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance);
    }

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
    }
}
