using System;
using System.Data.Common;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db parameter binding interface.
    /// </summary>
    internal interface IDbParameterBinding<TS> : IDbPropertyBinding<DbParameterAttribute>
        where TS : DbCommand
    {
        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(TS cmd, object instance);

        /// <summary>
        /// Get output parameters.
        /// </summary>
        void GetOutputParameters(TS cmd, object instance);
    }

    /// <summary>
    /// Db parameter property binding.
    /// </summary>
    internal class DbParameterBinding<TC, TP> : DbPropertyBinding<TC, TP, DbParameterAttribute>, IDbParameterBinding<TC>
        where TC : DbCommand
    {
        private readonly string parameterName;

        #region Constructor

        public DbParameterBinding(PropertyInfo propInfo, DbParameterAttribute attr, IDbNaming dbNaming)
            : base(propInfo, attr)
        {
            parameterName = Attributes.ParameterName;
            parameterName = dbNaming?.GetParameterName(propInfo, parameterName) ?? parameterName;

            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException($"Null parameter name in DbParameterBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(TC cmd, object instance)
        {
            try
            {
                switch (Mode)
                {
                    case BindingMode.Scalar:
                        DbHelper.SetSqlParameter(cmd, Attributes.ParameterName, GetValue(instance));
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
        public void GetOutputParameters(TC cmd, object instance)
        {
            try
            {
                switch (Mode)
                {
                    case BindingMode.Scalar:
                        SetValue(instance, DbHelper.GetSqlParameter(cmd, Attributes.ParameterName));
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
