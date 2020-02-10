using System;
using System.Data.SqlClient;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Class
{
    /// <summary>
    /// Db parameter binding interface.
    /// </summary>
    public interface IDbParameterBinding : IDbPropertyBinding<DbParameterAttribute>
    {
        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(SqlCommand cmd, object instance);

        /// <summary>
        /// Get output parameters.
        /// </summary>
        void GetOutputParameters(SqlCommand cmd, object instance);
    }

    /// <summary>
    /// Db parameter property binding.
    /// </summary>
    public class DbParameterBinding<TC, TP> : DbPropertyBinding<TC, TP, DbParameterAttribute>, IDbParameterBinding where TC : class
    {
        #region Constructor

        public DbParameterBinding(PropertyInfo propInfo, DbParameterAttribute attr)
            : base(propInfo, attr)
        {
        }

        #endregion

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(SqlCommand cmd, object instance)
        {
            try
            {
                switch (Mode)
                {
                    case BindingMode.Scalar:
                        DbHelper.SetSqlParameter(cmd, Attributes.DbName, GetValue(instance));
                        break;

                    case BindingMode.Class:
                    case BindingMode.List:
                    case BindingMode.Dictionary:
                    default:
                        throw new ArgumentException($"Property {this} of type [{PropertyInfo.PropertyType.Name}] has a unsupported binding {BindingType}.");
                }
            }
            catch
            {
                if (!Attributes.IsOptional)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get output parameters.
        /// </summary>
        public void GetOutputParameters(SqlCommand cmd, object instance)
        {
            try
            {
                switch (Mode)
                {
                    case BindingMode.Scalar:
                        SetValue(instance, DbHelper.GetSqlParameter(cmd, Attributes.DbName));
                        break;

                    case BindingMode.Class:
                    case BindingMode.List:
                    case BindingMode.Dictionary:
                    default:
                        throw new ArgumentException($"Property {this} of type [{PropertyInfo.PropertyType.Name}] has a unsupported binding {BindingType}.");
                }
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
