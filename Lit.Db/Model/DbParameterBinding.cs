using System;
using System.Reflection;

namespace Lit.Db
{
    /// <summary>
    /// Db parameter property binding.
    /// </summary>
    internal class DbParameterBinding<TC, TP> : DbPropertyBinding<TC, TP, DbParameterAttribute>, IDbParameterBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => true;

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        public string SpParamName { get; private set; }

        #region Constructor

        public DbParameterBinding(IDbTemplateBinding binding, PropertyInfo propInfo, DbParameterAttribute attr)
            : base(binding, propInfo, attr)
        {
            SpParamName = binding.Setup.Naming.GetParameterName(propInfo.Name, null, Attributes.ParameterName);

            if (string.IsNullOrEmpty(SpParamName))
            {
                throw new ArgumentException($"Null parameter name in DbParameterBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion
    }
}
