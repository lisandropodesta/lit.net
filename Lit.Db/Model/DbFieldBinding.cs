using System;
using System.Reflection;

namespace Lit.Db
{
    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbFieldBinding<TC, TP> : DbPropertyBinding<TC, TP, DbFieldAttribute>, IDbFieldBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => true;

        /// <summary>
        /// Field name.
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Field type.
        /// </summary>
        public Type FieldType => BindingType;

        /// <summary>
        /// Forced IsNullable value.
        /// </summary>
        protected override bool? IsNullableForced => Attributes.IsNullableForced;

        #region Constructor

        public DbFieldBinding(IDbSetup setup, PropertyInfo propInfo, DbFieldAttribute attr)
            : base(setup, propInfo, attr, false, true)
        {
            FieldName = setup.Naming.GetFieldName(propInfo, Attributes.DbName, KeyConstraint);

            if (string.IsNullOrEmpty(FieldName))
            {
                throw new ArgumentException($"Null field name in DbFieldBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion
    }
}
