using System;
using System.Collections.Generic;
using System.Text;
using Lit.DataType;
using Lit.Db.Attributes;
using Lit.Db.Custom.MySql;
using Lit.Db.Custom.MySql.Attributes;
using Lit.Db.Model;

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
        public CreateTable(Type tableTemplate, IDbSetup setup)
        {
            var tableAttr = TypeHelper.GetAttribute<MySqlTableAttribute>(tableTemplate, true);

            Engine = (tableAttr?.Engine ?? Custom.MySql.Engine.InnoDb).ToString();
            DefaultCharset = (tableAttr?.DefaultCharset ?? DefaultKey);

            var bindings = setup.GetTableBinding(tableTemplate);

            TableName = bindings.TableName;
            ColumnDefinition = GetColumnDefinition(bindings.Columns);
            TableConstraints = GetTableConstraints(bindings);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateTable(string tableName, IEnumerable<IDbColumnBinding> columns, Engine engine, string defaultCharset)
        {
            Engine = engine.ToString();
            DefaultCharset = defaultCharset;
            TableName = tableName;
            ColumnDefinition = GetColumnDefinition(columns);
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

                str.Append($"  `{col.FieldName}`");

                str.ConditionalAppend(" ", GetFieldType(col));
                str.ConditionalAppend(" ", GetNullable(col));
                str.ConditionalAppend(" ", GetDefault(col));
                str.ConditionalAppend(" ", GetAutoIncrement(col));

                first = false;
            }

            // Constraint definition
            foreach (var col in columns)
            {
                str.ConditionalAppend(",\n ", GetColumnIndex(col));
                str.ConditionalAppend(",\n ", GetColumnConstraint(col));
                str.ConditionalAppend(",\n ", GetColumnForeignKeyConstraint(col));
            }

            return str.ToString();
        }

        private string GetTableConstraints(IDbTableBinding table)
        {
            var str = new StringBuilder();

            if (table.PrimaryKey != null)
            {
                str.Append(",\n");
                str.Append($"{ConstraintKey} {PrimaryKeyKey} ( {GetKeyColumns(table, table.PrimaryKey, ", ", "`")} )");
            }

            if (table.ForeignKeys.Count > 0)
            {
                foreach (var fk in table.ForeignKeys)
                {
                    var name = fk.DbName ?? $"fk_{TableName}_{GetKeyColumns(table, fk, "_")}";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {name} {ForeignKeyKey} ( {GetKeyColumns(table, fk, ", ", "`")} ) {ReferecencesKey} {fk.ForeignTable} ( {ColumnsList(fk.ForeignColumns)} ) ON DELETE NO ACTION ON UPDATE NO ACTION");
                }
            }

            if (table.UniqueKeys.Count > 0)
            {
                foreach (var uk in table.UniqueKeys)
                {
                    var name = uk.DbName ?? $"uk_{TableName}_{GetKeyColumns(table, uk, "_")}_idx";
                    str.Append(",\n");
                    str.Append($"{ConstraintKey} {UniqueKeyKey} {name} ( {GetKeyColumns(table, uk, ", ", "`")} )");
                }
            }

            if (table.Indexes.Count > 0)
            {
                foreach (var idx in table.Indexes)
                {
                    var name = idx.DbName ?? $"{TableName}_{GetKeyColumns(table, idx, "_")}_idx";
                    str.Append(",\n");
                    str.Append($"{IndexKey} {name} ( {GetKeyColumns(table, idx, ", ", "`")} )");
                }
            }

            return str.ToString();
        }

        private string GetKeyColumns(IDbTableBinding table, IDbTableKeyAttribute key, string separator, string surround = null)
        {
            var columns = string.Empty;

            foreach (var name in key.FieldNames)
            {
                var col = table.FindColumn(name);
                if (col == null)
                {
                    throw new ArgumentException($"Invalid key column {name} in table [{table.TableName}]");
                }

                var fieldName = col.FieldName;
                columns += (!string.IsNullOrEmpty(columns) ? separator : string.Empty) + surround + fieldName + surround;
            }

            return columns;
        }

        private string ColumnsList(string[] columns)
        {
            var text = string.Empty;

            foreach (var name in columns)
            {
                text += (!string.IsNullOrEmpty(text) ? ", " : string.Empty) + "`" + name + "`";
            }

            return text;
        }

        private string GetFieldType(IDbColumnBinding column)
        {
            return MySqlDataType.Translate(column.DataType, column.FieldType, column.FieldSize);
        }

        private string GetNullable(IDbColumnBinding column)
        {
            return column.IsNullable ? NullKey : NotNullKey;
        }

        private string GetDefault(IDbColumnBinding column)
        {
            // TODO: implement this code!
            return string.Empty;
        }

        private string GetAutoIncrement(IDbColumnBinding column)
        {
            if (column.IsAutoIncrement)
            {
                return AutoIncrementKey;
            }

            return string.Empty;
        }

        private string GetColumnIndex(IDbColumnBinding column)
        {
            switch (column.KeyConstraint)
            {
                // {INDEX|KEY} [index_name] [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.PrimaryForeignKey:
                case DbKeyConstraint.ForeignKey:
                    return $"{KeyKey} fk_{TableName}_{column.FieldName}_idx ( {column.FieldName} )";

                case DbKeyConstraint.None:
                case DbKeyConstraint.PrimaryKey:
                case DbKeyConstraint.UniqueKey:
                default:
                    return string.Empty;
            }
        }

        private string GetColumnConstraint(IDbColumnBinding column)
        {
            switch (column.KeyConstraint)
            {
                // [CONSTRAINT [symbol]] PRIMARY KEY [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.PrimaryKey:
                case DbKeyConstraint.PrimaryForeignKey:
                    return $"{ConstraintKey} {PrimaryKeyKey} ( {column.FieldName} )";

                // [CONSTRAINT [symbol]] UNIQUE [INDEX|KEY] [index_name] [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.UniqueKey:
                    return $"{ConstraintKey} {UniqueKeyKey} uk_{TableName}_{column.FieldName} ( {column.FieldName} )";

                case DbKeyConstraint.ForeignKey:
                case DbKeyConstraint.None:
                default:
                    return string.Empty;
            }
        }

        private string GetColumnForeignKeyConstraint(IDbColumnBinding column)
        {
            switch (column.KeyConstraint)
            {
                // [CONSTRAINT [symbol]] FOREIGN KEY [index_name] (col_name,...)
                // REFERENCES tbl_name (key_part,...) [MATCH FULL | MATCH PARTIAL | MATCH SIMPLE] [ON DELETE reference_option] [ON UPDATE reference_option]
                case DbKeyConstraint.PrimaryForeignKey:
                case DbKeyConstraint.ForeignKey:
                    return $"{ConstraintKey} fk_{TableName}_{column.FieldName} {ForeignKeyKey} ( {column.FieldName} ) {ReferecencesKey} {column.ForeignTable} ( {column.ForeignColumn} ) ON DELETE NO ACTION ON UPDATE NO ACTION";

                case DbKeyConstraint.None:
                case DbKeyConstraint.PrimaryKey:
                case DbKeyConstraint.UniqueKey:
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
