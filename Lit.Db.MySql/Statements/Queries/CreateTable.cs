using System;
using System.Collections.Generic;
using System.Text;
using Lit.DataType;
using Lit.Db.Custom.MySql;

namespace Lit.Db.MySql.Statements
{
    /// <summary>
    /// Single table creation.
    /// </summary>
    [DbQuery(Template)]
    public class CreateTable : MySqlTemplate
    {
        public const string Template =
            "CREATE TABLE {{@table_name}} (\n" +
            "{{@column_definition}}\n" +
            "{{@table_constraints}}\n" +
            ") ENGINE={{@engine}} DEFAULT CHARSET={{@default_charset}}";

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
        /// Engine.
        /// </summary>
        [DbParameter("engine")]
        public string Engine { get; set; }

        /// <summary>
        /// Default charset.
        /// </summary>
        [DbParameter("default_charset")]
        public string DefaultCharset { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateTable(IDbSetup setup, Type tableTemplate) : base(setup)
        {
            var tableAttr = TypeHelper.GetAttribute<MySqlTableAttribute>(tableTemplate, true);

            Engine = (tableAttr?.Engine ?? Custom.MySql.Engine.InnoDb).ToString();
            DefaultCharset = (tableAttr?.DefaultCharset ?? DefaultKey);

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

                str.ConditionalAppend(" ", GetFieldType(col));
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

                // [CONSTRAINT [symbol]] PRIMARY KEY [index_type] (key_part,...) [index_option] ...
                str.Append(",\n");
                str.Append($"{ConstraintKey} {PrimaryKeyKey} ( {columnsList} )");
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

                    // [CONSTRAINT [symbol]] FOREIGN KEY [index_name] (col_name,...)
                    // REFERENCES tbl_name (key_part,...) [MATCH FULL | MATCH PARTIAL | MATCH SIMPLE] [ON DELETE reference_option] [ON UPDATE reference_option]
                    var name = fk.DbName ?? $"fk_{TableName}_{columnsName}";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {name} {ForeignKeyKey} ( {columnsList} ) ");
                    str.Append($"{ReferecencesKey} {pkBinding.GetSqlTableName()} ( {primaryList} ) ON DELETE NO ACTION ON UPDATE NO ACTION");

                    // {INDEX|KEY} [index_name] [index_type] (key_part,...) [index_option] ...
                    str.Append(",\n");
                    str.Append($"{KeyKey} fk_{TableName}_{columnsName}_idx ( {columnsList} )");
                }
            }

            if (table.UniqueKeys.Count > 0)
            {
                foreach (var uk in table.UniqueKeys)
                {
                    var columnsName = table.AggregateText(uk, "_", c => c.ColumnName);
                    var columnsList = table.AggregateText(uk, ", ", c => c.GetSqlColumnName());

                    // [CONSTRAINT [symbol]] UNIQUE [INDEX|KEY] [index_name] [index_type] (key_part,...) [index_option] ...
                    var name = uk.DbName ?? $"uk_{TableName}_{columnsName}_idx";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {UniqueKeyKey} {name} ( {columnsList} )");
                }
            }

            if (table.Indexes.Count > 0)
            {
                foreach (var idx in table.Indexes)
                {
                    var columnsName = table.AggregateText(idx, "_", c => c.ColumnName);
                    var columnsList = table.AggregateText(idx, ", ", c => c.GetSqlColumnName());

                    // {INDEX|KEY} [index_name] [index_type] (key_part,...) [index_option] ...
                    var name = idx.DbName ?? $"{TableName}_{columnsName}_idx";
                    str.Append(",\n");
                    str.Append($"{IndexKey} {name} ( {columnsList} )");
                }
            }

            return str.ToString();
        }

        #endregion
    }
}
