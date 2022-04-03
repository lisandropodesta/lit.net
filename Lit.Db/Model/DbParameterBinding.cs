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

        public DbParameterBinding(IDbSetup setup, PropertyInfo propInfo, DbParameterAttribute attr)
            : base(setup, propInfo, attr, attr.IsInput, attr.IsOutput)
        {
            SpParamName = setup.Naming.GetParameterName(propInfo, null, Attributes.ParameterName, attr.DoNotTranslate, KeyConstraint);

            if (string.IsNullOrEmpty(SpParamName))
            {
                throw new ArgumentException($"Null parameter name in DbParameterBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion
    }
}
