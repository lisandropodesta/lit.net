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
        void LoadResults(DbDataReader reader, object instance, IDbNaming dbNaming);
    }

    /// <summary>
    /// Db recordset property binding.
    /// </summary>
    internal class DbRecordsetBinding<TS, TC, TP> : DbPropertyBinding<TC, TP, DbRecordsetAttribute>, IDbRecordsetBinding
        where TS : DbCommand
        where TC : class
    {
        #region Constructor

        public DbRecordsetBinding(PropertyInfo propInfo, DbRecordsetAttribute attr, IDbNaming dbNaming)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public void LoadResults(DbDataReader reader, object instance, IDbNaming dbNaming)
        {
            var list = DbHelper.LoadSqlRecordset<TS>(reader, BindingType, int.MaxValue, dbNaming);
            SetValue(instance, list);
        }
    }
}
