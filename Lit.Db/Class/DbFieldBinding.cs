using System.Data.SqlClient;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Class
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbFieldBinding : IDbPropertyBinding<DbFieldAttribute>
    {
        /// <summary>
        /// Get output field.
        /// </summary>
        void GetOutputField(SqlDataReader reader, object instance);
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    public class DbFieldBinding<TC, TP> : DbPropertyBinding<TC, TP, DbFieldAttribute>, IDbFieldBinding where TC : class
    {
        #region Constructor

        public DbFieldBinding(PropertyInfo propInfo, DbFieldAttribute attr)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Get output field.
        /// </summary>
        public void GetOutputField(SqlDataReader reader, object instance)
        {
            try
            {
                SetValue(instance, reader[Attributes.DbName]);
            }
            catch
            {
                if (!Attributes.IsOptional)
                {
                    throw;
                }
            }
        }
    }
}
