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
    public interface IDbParameterBinding : IDbPropertyBinding<DbParameterAttribute>
    {
        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(ref string text, object instance);

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);

        /// <summary>
        /// Get output parameters.
        /// </summary>
        void GetOutputParameters(DbCommand cmd, object instance);
    }

    /// <summary>
    /// Db parameter property binding.
    /// </summary>
    internal class DbParameterBinding<TC, TP> : DbPropertyBinding<TC, TP, DbParameterAttribute>, IDbParameterBinding
        where TC : class
    {
        #region Constructor

        public DbParameterBinding(IDbSetup setup, PropertyInfo propInfo, DbParameterAttribute attr)
            : base(setup, propInfo, attr)
        {
            spParamName = setup.Naming.GetParameterName(propInfo.Name, Attributes.ParameterName);

            if (string.IsNullOrEmpty(spParamName))
            {
                throw new ArgumentException($"Null parameter name in DbParameterBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        public string SpParamName => spParamName;

        private readonly string spParamName;

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(ref string text, object instance)
        {
            if (Attributes.IsInput)
            {
                switch (Mode)
                {
                    case BindingMode.Scalar:
                        var value = GetValue(instance);
                        DbHelper.SetSqlParameter(ref text, spParamName, value.ToString(), Attributes.IsOptional);
                        break;

                    case BindingMode.Class:
                    case BindingMode.List:
                    case BindingMode.Dictionary:
                    default:
                        throw new ArgumentException($"Property {this} of type [{PropertyInfo.PropertyType.Name}] has a unsupported binding {BindingType}.");
                }
            }
        }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(DbCommand cmd, object instance)
        {
            if (Attributes.IsInput)
            {
                try
                {
                    switch (Mode)
                    {
                        case BindingMode.Scalar:
                            DbHelper.SetSqlParameter(cmd, spParamName, GetValue(instance), Attributes.IsOptional);
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

        /// <summary>
        /// Get output parameters.
        /// </summary>
        public void GetOutputParameters(DbCommand cmd, object instance)
        {
            if (Attributes.IsOutput)
            {
                try
                {
                    switch (Mode)
                    {
                        case BindingMode.Scalar:
                            SetValue(instance, DbHelper.GetSqlParameter(cmd, spParamName));
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
}
