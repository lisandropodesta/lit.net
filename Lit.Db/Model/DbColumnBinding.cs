using System;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
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
        /// Field size.
        /// </summary>
        public ulong? FieldSize => fieldSize;

        private readonly ulong? fieldSize;

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
            fieldSize = attr.Size;

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException($"Null field name in DbColumnBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }

            isAutoIncrement = attr.AutoIncrement;

            if (attr is DbPrimaryKeyAttribute)
            {
                if (IsNullable)
                {
                    throw new ArgumentException($"Primary key can not be nullable on property [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}.{propInfo.Name}]");
                }

                if (attr is DbPrimaryAndForeignKeyAttribute)
                {
                    keyConstraint = DbKeyConstraint.PrimaryForeignKey;
                }
                else
                {
                    keyConstraint = DbKeyConstraint.PrimaryKey;
                }
            }
            else if (attr is DbForeignKeyAttribute)
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
        /// Assign foreign key.
        /// </summary>
        public void AssignForeignKey(string foreignTable, string foreignColumn)
        {
            this.foreignTable = foreignTable;
            this.foreignColumn = foreignColumn;
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