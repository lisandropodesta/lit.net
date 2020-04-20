using System;
using System.Data.Common;
using System.Reflection;

namespace Lit.Db
{
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
                var value = GetValue(instance);
                DbHelper.SetSqlParameter(ref text, spParamName, value.ToString(), Attributes.IsOptional);
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
                    DbHelper.SetSqlParameter(cmd, spParamName, GetValue(instance), Attributes.IsOptional);
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
                    SetValue(instance, DbHelper.GetSqlParameter(cmd, spParamName));
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
