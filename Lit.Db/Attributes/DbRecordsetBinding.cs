using System.Data.SqlClient;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Class
{
    /// <summary>
    /// Db recordset binding interface.
    /// </summary>
    public interface IDbRecordsetBinding : IDbPropertyBinding<DbRecordsetAttribute>
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(SqlDataReader reader, object instance);
    }

    /// <summary>
    /// Db recordset property binding.
    /// </summary>
    public class DbRecordsetBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordsetAttribute>, IDbRecordsetBinding where TC : class
    {
        #region Constructor

        public DbRecordsetBinding(PropertyInfo propInfo, DbRecordsetAttribute attr)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(SqlDataReader reader, object instance)
        {
            var list = DbHelper.LoadSqlRecordset(reader, BindingType, int.MaxValue);
            SetValue(instance, list);
        }
    }
}
