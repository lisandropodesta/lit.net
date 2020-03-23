using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Table template information.
    /// </summary>
    internal class DbTableBinding : DbTemplateBinding, IDbTableBinding
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public string TableName => tableName;

        private readonly string tableName;

        /// <summary>
        /// Columns.
        /// </summary>
        public IReadOnlyList<IDbColumnBinding> Columns => columnBindings;

        private readonly List<IDbColumnBinding> columnBindings;

        /// <summary>
        /// Primary key.
        /// </summary>
        public IDbTablePrimaryKeyAttribute PrimaryKey => primaryKey;

        private readonly IDbTablePrimaryKeyAttribute primaryKey;

        /// <summary>
        /// Foreign keys.
        /// </summary>
        public IReadOnlyList<IDbTableForeignKeyAttribute> ForeignKeys => foreignKeys;

        private readonly List<IDbTableForeignKeyAttribute> foreignKeys;

        /// <summary>
        /// Unique keys.
        /// </summary>
        public IReadOnlyList<IDbTableUniqueKeyAttribute> UniqueKeys => uniqueKeys;

        private readonly List<IDbTableUniqueKeyAttribute> uniqueKeys;

        /// <summary>
        /// Indexes.
        /// </summary>
        public IReadOnlyList<IDbTableIndexAttribute> Indexes => indexes;

        private readonly List<IDbTableIndexAttribute> indexes;

        #region Constructor

        internal DbTableBinding(Type templateType, IDbSetup setup)
            : base(templateType, setup)
        {
            var tattr = TypeHelper.GetAttribute<DbTableAttribute>(templateType, true);

            if (tattr != null)
            {
                tableName = setup.Naming.GetTableName(templateType, tattr.TableName);
            }

            primaryKey = TypeHelper.GetAttribute<IDbTablePrimaryKeyAttribute>(templateType, true);
            foreignKeys = TypeHelper.GetAttributes<IDbTableForeignKeyAttribute>(templateType, true).ToList();
            uniqueKeys = TypeHelper.GetAttributes<IDbTableUniqueKeyAttribute>(templateType, true).ToList();
            indexes = TypeHelper.GetAttributes<IDbTableIndexAttribute>(templateType, true).ToList();

            foreach (var propInfo in templateType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (TypeHelper.GetAttribute<DbColumnAttribute>(propInfo, out var cAttr))
                {
                    AddBinding(ref columnBindings, typeof(DbColumnBinding<,>), propInfo, cAttr, setup);
                }
            }
        }

        #endregion

        /// <summary>
        /// Resolve foreign keys.
        /// </summary>
        public void ResolveForeignKeys()
        {
            columnBindings?.ForEach(binding => ResolveForeignKey(binding));
            foreignKeys?.ForEach(fk => ResolveForeignKey(fk));
        }

        /// <summary>
        /// Finds a column binding.
        /// </summary>
        public IDbColumnBinding FindColumn(string propertyName)
        {
            var propInfo = TemplateType.GetProperty(propertyName);
            return Columns?.FirstOrDefault(i => i.PropertyInfo == propInfo);
        }

        /// <summary>
        /// Get output fields.
        /// </summary>
        public void GetOutputFields(DbDataReader reader, object instance)
        {
            columnBindings?.ForEach(c => c.GetOutputField(reader, instance));
        }

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        public bool LoadResults(DbDataReader reader, object instance)
        {
            if (reader.Read())
            {
                GetOutputFields(reader, instance);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the first column that matches with selection.
        /// </summary>
        public IDbColumnBinding FindFirstColumn(DbColumnsSelection selection)
        {
            return Columns.FirstOrDefault(c => IsSelected(c, selection));
        }

        /// <summary>
        /// Maps an action to every column.
        /// </summary>
        public void MapColumns(DbColumnsSelection selection, Action<IDbColumnBinding> action)
        {
            Columns.Where(c => IsSelected(c, selection)).ForEach(c => action(c));
        }

        /// <summary>
        /// Check whether if a column matches a selection criteria or not.
        /// </summary>
        public static bool IsSelected(IDbColumnBinding column, DbColumnsSelection selection)
        {
            switch (selection)
            {
                case DbColumnsSelection.None:
                    return false;

                case DbColumnsSelection.PrimaryKey:
                    return column.KeyConstraint == DbKeyConstraint.PrimaryKey || column.KeyConstraint == DbKeyConstraint.PrimaryForeignKey;

                case DbColumnsSelection.UniqueKey:
                    return column.KeyConstraint == DbKeyConstraint.UniqueKey;

                case DbColumnsSelection.NonPrimaryKey:
                    return column.KeyConstraint != DbKeyConstraint.PrimaryKey && column.KeyConstraint != DbKeyConstraint.PrimaryForeignKey;

                case DbColumnsSelection.All:
                default:
                    return true;
            }
        }

        /// <summary>
        /// Resolve a single column foreign key.
        /// </summary>
        private void ResolveForeignKey(IDbColumnBinding col)
        {
            if (col.Attributes is IDbForeignKeyAttribute fk)
            {
                var binding = Setup.GetTableBinding(fk.ForeignTableTemplate);
                var colBinding = !string.IsNullOrEmpty(fk.ForeignColumnProperty) ? binding.FindColumn(fk.ForeignColumnProperty)
                    : binding.FindFirstColumn(DbColumnsSelection.PrimaryKey);

                if (colBinding == null)
                {
                    throw new ArgumentException($"Invalid foreign key definition [{col.PropertyInfo.DeclaringType.Namespace}.{col.PropertyInfo.DeclaringType.Name}.{col.PropertyInfo.Name}]");
                }

                var foreignTable = binding.TableName;
                var foreignColumn = Setup.Naming.GetColumnName(foreignTable, colBinding.PropertyInfo, colBinding.FieldName);
                col.AssignForeignKey(foreignTable, foreignColumn);
            }
        }

        /// <summary>
        /// Resolve a multi column foreign key.
        /// </summary>
        private void ResolveForeignKey(IDbTableForeignKeyAttribute fk)
        {
            var binding = Setup.GetTableBinding(fk.ForeignTableTemplate);

            if (binding.PrimaryKey == null)
            {
                throw new ArgumentException($"Invalid foreign key definition in table [{TableName}], remote table has no primary key");
            }

            if (binding.PrimaryKey.FieldNames.Length != fk.FieldNames.Length)
            {
                throw new ArgumentException($"Invalid foreign key definition in table [{TableName}], local fields count [{fk.FieldNames.Length}] do not match with foreign fields count [{binding.PrimaryKey.FieldNames.Length}]");
            }

            var foreignTable = binding.TableName;
            var foreignColumns = new List<string>();

            foreach (var colName in binding.PrimaryKey.FieldNames)
            {
                var colBinding = binding.FindColumn(colName);
                foreignColumns.Add(Setup.Naming.GetColumnName(foreignTable, colBinding.PropertyInfo, colBinding.FieldName));
            }

            fk.ForeignTable = foreignTable;
            fk.ForeignColumns = foreignColumns.ToArray();
        }
    }
}
