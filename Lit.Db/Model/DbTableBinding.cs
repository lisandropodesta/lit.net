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
    public class DbTableBinding : DbTemplateBinding
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

        #region Constructor

        internal DbTableBinding(Type templateType, IDbSetup setup)
            : base(templateType, setup)
        {
            var tattr = TypeHelper.GetAttribute<DbTableAttribute>(templateType, true);

            if (tattr != null)
            {
                tableName = setup.Naming.GetTableName(templateType, tattr.TableName);
            }

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
        internal void ResolveForeignKeys()
        {
            columnBindings?.ForEach(binding => binding.ResolveForeignKey());
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
        public void LoadResults(DbDataReader reader, object instance)
        {
            if (reader.Read())
            {
                GetOutputFields(reader, instance);
            }
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
                    return column.KeyConstraint == DbKeyConstraint.PrimaryKey;

                case DbColumnsSelection.UniqueKey:
                    return column.KeyConstraint == DbKeyConstraint.UniqueKey;

                case DbColumnsSelection.NonPrimaryKey:
                    return column.KeyConstraint != DbKeyConstraint.PrimaryKey;

                case DbColumnsSelection.All:
                default:
                    return true;
            }
        }
    }
}
