using System;
using System.Collections.Generic;
using System.Text;

namespace Lit.Db.PostgreSQL.Statements
{
    /// <summary>
    /// Single table creation.
    /// </summary>
    [DbQuery(Template)]
    public class CreateTable : PostgresTemplate
    {
        public const string Template =
            "CREATE TABLE {{@table_name}} (\n" +
            "{{@column_definition}}\n" +
            "{{@table_constraints}}\n" +
            ")";

        /// <summary>
        /// Table name.
        /// </summary>
        [DbParameter("table_name")]
        public string TableName { get; set; }

        /// <summary>
        /// Column definition.
        /// </summary>
        [DbParameter("column_definition")]
        protected string ColumnDefinition { get; set; }

        /// <summary>
        /// Table constraints.
        /// </summary>
        [DbParameter("table_constraints")]
        protected string TableConstraints { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateTable(IDbSetup setup, Type tableTemplate) : base(setup)
        {
            var bindings = setup.GetTableBinding(tableTemplate);

            TableName = bindings.TableName;
            ColumnDefinition = GetColumnDefinition(bindings.Columns);
            TableConstraints = GetTableConstraints(bindings);
        }

        #region Property setters

        private string GetColumnDefinition(IEnumerable<IDbColumnBinding> columns)
        {
            var str = new StringBuilder();

            // Column definition
            var first = true;
            foreach (var col in columns)
            {
                if (!first)
                {
                    str.Append(",\n");
                }

                str.Append($"  {col.GetSqlColumnName()}");

                str.ConditionalAppend(" ", col.GetSqlColumnType());
                str.ConditionalAppend(" ", GetNullable(col));
                str.ConditionalAppend(" ", GetDefault(col));
                str.ConditionalAppend(" ", GetAutoIncrement(col));

                first = false;
            }

            return str.ToString();
        }

        private string GetTableConstraints(IDbTableBinding table)
        {
            var str = new StringBuilder();

            if (table.PrimaryKey != null)
            {
                var columnsList = table.AggregateText(table.PrimaryKey, ", ", c => c.GetSqlColumnName());

                str.Append(",\n");
                str.Append($"{ConstraintKey} {table.TableName}_pkey {PrimaryKeyKey} ( {columnsList} )");
            }

            if (table.ForeignKeys.Count > 0)
            {
                table.CheckConstraints();

                foreach (var fk in table.ForeignKeys)
                {
                    var columnsName = table.AggregateText(fk, "_", c => c.ColumnName);
                    var columnsList = table.AggregateText(fk, ", ", c => c.GetSqlColumnName());
                    var pkBinding = Setup.GetTableBinding(fk.PrimaryTableTemplate);
                    var primaryList = pkBinding.AggregateText(DbColumnsSelection.PrimaryKey, ", ", c => c.GetSqlColumnName());

                    var name = fk.DbName ?? $"fk_{TableName}_{columnsName}";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {name} {ForeignKeyKey} ( {columnsList} ) ");
                    str.Append($"{ReferencesKey} {pkBinding.GetSqlTableName()} ( {primaryList} ) ON DELETE NO ACTION ON UPDATE NO ACTION");
                }
            }

            if (table.UniqueKeys.Count > 0)
            {
                foreach (var uk in table.UniqueKeys)
                {
                    var columnsName = table.AggregateText(uk, "_", c => c.ColumnName);
                    var columnsList = table.AggregateText(uk, ", ", c => c.GetSqlColumnName());

                    var name = uk.DbName ?? $"uk_{TableName}_{columnsName}";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {name} {UniqueKeyKey} ( {columnsList} )");
                }
            }

            if (table.Indexes.Count > 0)
            {
                foreach (var idx in table.Indexes)
                {
                    var columnsName = table.AggregateText(idx, "_", c => c.ColumnName);
                    var columnsList = table.AggregateText(idx, ", ", c => c.GetSqlColumnName());

                    var name = idx.DbName ?? $"{TableName}_{columnsName}_idx";
                    str.Append(",\n");
                    str.Append($"CREATE {IndexKey} {name} ON {TableName} ( {columnsList} )");
                }
            }

            return str.ToString();
        }

        #endregion
    }
}
