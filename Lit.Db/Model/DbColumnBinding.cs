using System;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbColumnBinding : IDbPropertyBinding<DbColumnAttribute>
    {
        /// <summary>
        /// Field name.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Field type.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Nullable flag.
        /// </summary>
        bool IsNullable { get; }

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

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }

        /// <summary>
        /// Resolve foreign key.
        /// </summary>
        void ResolveForeignKey();

        /// <summary>
        /// Get output field.
        /// </summary>
        void GetOutputField(DbDataReader reader, object instance);

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbColumnBinding<TC, TP> : DbPropertyBinding<TC, TP, DbColumnAttribute>, IDbColumnBinding
        where TC : class
    {
        /// <summary>
        /// Field name.
        /// </summary>
        public string FieldName => fieldName;

        private readonly string fieldName;

        /// <summary>
        /// Field type.
        /// </summary>
        public Type FieldType => BindingType;

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

        private string foreignTable;

        /// <summary>
        /// Foreign column.
        /// </summary>
        public string ForeignColumn => foreignColumn;

        private string foreignColumn;

        /// <summary>
        /// Name of the related stored procedure parameter.
        /// </summary>
        public string SpParamName => spParamName;

        private readonly string spParamName;

        #region Constructor

        public DbColumnBinding(IDbSetup setup, PropertyInfo propInfo, DbColumnAttribute attr)
            : base(setup, propInfo, attr, attr.IsNullableDefined ? (bool?)attr.IsNullable : null)
        {
            fieldName = setup.Naming.GetFieldName(propInfo.Name, Attributes.DbName);

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException($"Null field name in DbColumnBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }

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
            }
            else if (attr is DbUniqueKeyAttribute)
            {
                keyConstraint = DbKeyConstraint.UniqueKey;
            }
            else
            {
                keyConstraint = DbKeyConstraint.None;
            }

            spParamName = setup.Naming.GetParameterName(FieldName, null);
        }

        #endregion

        /// <summary>
        /// Resolve foreign key.
        /// </summary>
        public void ResolveForeignKey()
        {
            if (Attributes is DbForeignKeyAttribute fk)
            {
                var binding = Setup.GetTableBinding(fk.ForeignTableTemplate);
                var colBinding = !string.IsNullOrEmpty(fk.ForeignColumnProperty) ? binding.FindColumn(fk.ForeignColumnProperty)
                    : binding.FindFirstColumn(DbColumnsSelection.PrimaryKey);

                if (colBinding == null)
                {
                    throw new ArgumentException($"Invalid foreign key definition [{PropertyInfo.DeclaringType.Namespace}.{PropertyInfo.DeclaringType.Name}.{PropertyInfo.Name}]");
                }

                foreignTable = binding.TableName;
                foreignColumn = Setup.Naming.GetColumnName(foreignTable, colBinding.PropertyInfo, colBinding.FieldName);
            }
        }

        /// <summary>
        /// Get output field.
        /// </summary>
        public void GetOutputField(DbDataReader reader, object instance)
        {
            SetValue(instance, reader[fieldName]);
        }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(DbCommand cmd, object instance)
        {
            DbHelper.SetSqlParameter(cmd, SpParamName, GetValue(instance), false);
        }
    }
}