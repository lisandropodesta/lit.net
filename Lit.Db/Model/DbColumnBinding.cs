using System;
using System.Reflection;

namespace Lit.Db
{
    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbColumnBinding<TC, TP> : DbPropertyBinding<TC, TP, DbColumnAttribute>, IDbColumnBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => true;

        /// <summary>
        /// Column name.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Column type.
        /// </summary>
        public Type ColumnType => BindingType;

        /// <summary>
        /// Column size.
        /// </summary>
        public ulong? ColumnSize { get; private set; }

        /// <summary>
        /// Key constraint.
        /// </summary>
        public DbKeyConstraint KeyConstraint { get; private set; }

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        public bool IsAutoIncrement => Attributes.AutoIncrement;

        /// <summary>
        /// Name of the related stored procedure parameter.
        /// </summary>
        public string SpParamName { get; private set; }

        /// <summary>
        /// Forced IsNullable value.
        /// </summary>
        protected override bool? IsNullableForced => Attributes.IsNullableForced;

        #region Constructor

        public DbColumnBinding(IDbTemplateBinding binding, PropertyInfo propInfo, DbColumnAttribute attr)
            : base(binding, propInfo, attr)
        {
            GetKeyConstraints();

            var setup = binding.Setup;
            ColumnName = setup.Naming.GetColumnName((binding as IDbTableBinding)?.TableName, propInfo, Attributes.DbName);
            ColumnSize = attr.Size;

            if (string.IsNullOrEmpty(ColumnName))
            {
                throw new ArgumentException($"Null field name in DbColumnBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }

            SpParamName = setup.Naming.GetParameterName(propInfo.Name, ColumnName, null);
        }

        #endregion

        /// <summary>
        /// Get key constraints.
        /// </summary>
        private void GetKeyConstraints()
        {
            var attr = Attributes;
            if (attr is DbPrimaryKeyAttribute)
            {
                if (IsNullable)
                {
                    throw new ArgumentException($"Primary key can not be nullable on property [{PropertyInfo.DeclaringType.Namespace}.{PropertyInfo.DeclaringType.Name}.{PropertyInfo.Name}]");
                }

                if (attr is DbPrimaryAndForeignKeyAttribute pfk)
                {
                    KeyConstraint = DbKeyConstraint.PrimaryForeignKey;
                    PrimaryTableTemplate = pfk.PrimaryTableTemplate;
                }
                else if (IsForeignKeyProp)
                {
                    KeyConstraint = DbKeyConstraint.ForeignKey;
                }
                else
                {
                    KeyConstraint = DbKeyConstraint.PrimaryKey;
                }
            }
            else if (attr is DbForeignKeyAttribute fk)
            {
                KeyConstraint = DbKeyConstraint.ForeignKey;
                PrimaryTableTemplate = fk.PrimaryTableTemplate;
            }
            else if (IsForeignKeyProp)
            {
                KeyConstraint = DbKeyConstraint.ForeignKey;
            }
            else if (attr is DbUniqueKeyAttribute)
            {
                KeyConstraint = DbKeyConstraint.UniqueKey;
            }
            else
            {
                KeyConstraint = DbKeyConstraint.None;
            }
        }
   }
}
