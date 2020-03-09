using System;
using System.Data.Common;
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

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }

        /// <summary>
        /// Resolve foreign key.
        /// </summary>
        void ResolveForeignKey();

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);
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
            : base(setup, propInfo, attr)
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
        /// Assigns input parameters.
        /// </summary>
        public void SetInputParameters(DbCommand cmd, object instance)
        {
            DbHelper.SetSqlParameter(cmd, SpParamName, GetValue(instance), false);
        }
    }
}