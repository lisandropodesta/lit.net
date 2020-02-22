using System;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbColumnBinding : IDbFieldBinding
    {
        /// <summary>
        /// Key constraint.
        /// </summary>
        DbKeyConstraint KeyConstraint { get; }

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        bool IsAutoIncrement { get; }

        /// <summary>
        /// Foreign table.
        /// </summary>
        string ForeignTable { get; }

        /// <summary>
        /// Foreign column.
        /// </summary>
        string ForeignColumn { get; }
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbColumnBinding<TC, TP> : DbFieldBinding<TC, TP>, IDbColumnBinding
        where TC : class
    {
        /// <summary>
        /// Key constraint.
        /// </summary>
        public DbKeyConstraint KeyConstraint => keyConstraint;

        private readonly DbKeyConstraint keyConstraint;

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        public bool IsAutoIncrement => isAutoIncrement;

        private readonly bool isAutoIncrement;

        /// <summary>
        /// Foreign table.
        /// </summary>
        public string ForeignTable => foreignTable;

        private readonly string foreignTable;

        /// <summary>
        /// Foreign column.
        /// </summary>
        public string ForeignColumn => foreignColumn;

        private readonly string foreignColumn;

        #region Constructor

        public DbColumnBinding(PropertyInfo propInfo, DbFieldAttribute attr, IDbNaming dbNaming)
            : base(propInfo, attr, dbNaming)
        {
            if (attr is DbPrimaryKeyAttribute pk)
            {
                keyConstraint = DbKeyConstraint.PrimaryKey;
                isAutoIncrement = pk.AutoIncrement;

                if (IsNullable)
                {
                    throw new ArgumentException($"Primary key can not be nullable on property [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}.{propInfo.Name}]");
                }
            }
            else if (attr is DbForeignKeyAttribute fk)
            {
                keyConstraint = DbKeyConstraint.ForeignKey;

                var binding = DbTemplateBinding.Get(fk.ForeignTableTemplate, dbNaming);
                var colBinding = binding?.FindColumn(fk.ForeignColumnProperty);

                if (colBinding == null)
                {
                    throw new ArgumentException($"Invalid foreign key definition [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}.{propInfo.Name}]");
                }

                foreignTable = binding.Text;
                foreignColumn = dbNaming.GetColumnName(foreignTable, colBinding.PropertyInfo, colBinding.FieldName);
            }
            else if (attr is DbUniqueKeyAttribute)
            {
                keyConstraint = DbKeyConstraint.UniqueKey;
            }
            else
            {
                keyConstraint = DbKeyConstraint.None;
            }
        }

        #endregion
    }
}