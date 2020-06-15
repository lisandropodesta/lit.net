using System;
using System.Collections.Generic;
using System.Linq;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Table template information.
    /// </summary>
    internal class DbTableBinding : DbTemplateBinding, IDbTableBinding
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Columns.
        /// </summary>
        public IReadOnlyList<IDbColumnBinding> Columns => columns.BindingList;

        private ITypeBinding<IDbColumnBinding> columns;

        /// <summary>
        /// Single column primary key.
        /// </summary>
        public IDbColumnBinding SingleColumnPrimaryKey { get; private set; }

        /// <summary>
        /// Single column unique key.
        /// </summary>
        public IDbColumnBinding SingleColumnUniqueKey { get; private set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        public IDbTablePrimaryKeyAttribute PrimaryKey { get; private set; }

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
            var tattr = TypeHelper.TryGetAttribute<DbTableAttribute>(templateType, true);

            if (tattr != null)
            {
                TableName = setup.Naming.GetTableName(templateType, tattr.TableName);
            }

            PrimaryKey = TypeHelper.TryGetAttribute<IDbTablePrimaryKeyAttribute>(templateType, true);
            foreignKeys = TypeHelper.GetAttributes<IDbTableForeignKeyAttribute>(templateType, true).ToList();
            uniqueKeys = TypeHelper.GetAttributes<IDbTableUniqueKeyAttribute>(templateType, true).ToList();
            indexes = TypeHelper.GetAttributes<IDbTableIndexAttribute>(templateType, true).ToList();

            AddColumns();
        }

        #endregion

        /// <summary>
        /// Scan properties and add column definition.
        /// </summary>
        private void AddColumns()
        {
            List<string> primaryKeyProps = null;
            Dictionary<Type, List<IDbColumnBinding>> foreignKeyFields = null;
            List<string> uniqueKeyProps = null;

            columns = new TypeBinding<IDbColumnBinding, DbColumnAttribute>(TemplateType,
                (p, a) => TypeHelper.CreateInstance(typeof(DbColumnBinding<,>), new[] { p.DeclaringType, p.PropertyType }, this, p, a) as IDbColumnBinding);

            foreach (var col in columns.BindingList)
            {
                var keyConstraint = col.KeyConstraint;

                if (keyConstraint == DbKeyConstraint.PrimaryKey || keyConstraint == DbKeyConstraint.PrimaryForeignKey)
                {
                    (primaryKeyProps = primaryKeyProps ?? new List<string>()).Add(col.PropertyName);
                }

                if (keyConstraint == DbKeyConstraint.UniqueKey)
                {
                    (uniqueKeyProps = uniqueKeyProps ?? new List<string>()).Add(col.PropertyName);
                }

                if (keyConstraint == DbKeyConstraint.ForeignKey || keyConstraint == DbKeyConstraint.PrimaryForeignKey)
                {
                    foreignKeyFields = foreignKeyFields ?? new Dictionary<Type, List<IDbColumnBinding>>();
                    if (!foreignKeyFields.TryGetValue(col.PrimaryTableTemplate, out List<IDbColumnBinding> list))
                    {
                        list = new List<IDbColumnBinding>();
                        foreignKeyFields.Add(col.PrimaryTableTemplate, list);
                    }
                    list.Add(col);
                }
            }

            if (primaryKeyProps != null)
            {
                if (PrimaryKey != null)
                {
                    throw new ArgumentException($"Multiple primary key definition in table [{TableName}]");
                }

                PrimaryKey = new DbTablePrimaryKeyAttribute(primaryKeyProps.ToArray());
            }

            if (PrimaryKey?.PropertyNames.Length == 1)
            {
                SingleColumnPrimaryKey = this.FindColumn(primaryKeyProps[0]);
            }

            if (uniqueKeyProps != null)
            {
                uniqueKeys.Add(new DbTableUniqueKeyAttribute(uniqueKeyProps.ToArray()));
            }

            if (uniqueKeys?.Count == 1 && uniqueKeys[0].PropertyNames.Length == 1)
            {
                SingleColumnUniqueKey = this.FindColumn(uniqueKeys[0].PropertyNames[0]);
            }

            if (foreignKeyFields != null)
            {
                foreach (var item in foreignKeyFields)
                {
                    foreach (var rel in item.Value)
                    {
                        var attr = new DbTableForeignKeyAttribute(item.Key, rel.PropertyName);
                        foreignKeys.Add(attr);
                    }
                }
            }
        }

        /// <summary>
        /// Calculate binding.
        /// </summary>
        internal void ResolveBinding()
        {
            columns.BindingList.ForEach(col => col.CalcBindingMode());
        }
    }
}
