using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Lit.Db.Attributes;
using Lit.Db.Model;
using Lit.Db.MySql.Schema.Information;

namespace Lit.Db.MySql.Statements
{
    /// <summary>
    /// Single table creation.
    /// </summary>
    [DbQuery(Template)]
    public class CreateTable : DbTemplate
    {
        public const string Template =
            "CREATE TABLE {{@table_name}} (\n" +
            "{{@column_definition}}\n" +
            "{{@table_constraints}}\n" +
            ") ENGINE={{@engine}} DEFAULT CHARSET={{@default_charset}}";

        public const string NullKey = "NULL";

        public const string NotNullKey = "NOT NULL";

        public const string AutoIncrementKey = "AUTO_INCREMENT";

        public const string KeyKey = "KEY";

        public const string IndexKey = "INDEX";

        public const string ConstraintKey = "CONSTRAINT";

        public const string PrimaryKeyKey = "PRIMARY KEY";

        public const string ForeignKeyKey = "FOREIGN KEY";

        public const string UniqueKeyKey = "UNIQUE KEY";

        public const string ReferecencesKey = "REFERENCES";

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
        /// Statemente execution.
        /// </summary>
        public CreateTable(Type tableTemplate, IDbNaming dbNaming, Engine engine, string defaultCharset)
        {
            Engine = engine.ToString();
            DefaultCharset = defaultCharset;

            var bindings = DbTemplateCache.Get(tableTemplate, dbNaming);
            if (bindings.CommandType != CommandType.TableDirect)
            {
                throw new ArgumentException($"Invalid table template for type {tableTemplate}");
            }

            TableName = bindings.Text;
            ColumnDefinition = GetColumnDefinition(bindings.Columns);
            TableConstraints = GetTableConstraints(bindings.Columns);
        }

        /// <summary>
        /// Statemente execution.
        /// </summary>
        public CreateTable(string tableName, IEnumerable<IDbColumnBinding> columns, Engine engine, string defaultCharset)
        {
            Engine = engine.ToString();
            DefaultCharset = defaultCharset;
            TableName = tableName;
            ColumnDefinition = GetColumnDefinition(columns);
            TableConstraints = GetTableConstraints(columns);
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
                str.ConditionalAppend(",\n ", GetColumnConstraints(col));
            }

            return str.ToString();
        }

        private string GetTableConstraints(IEnumerable<IDbColumnBinding> columns)
        {
            var str = new StringBuilder();

            // TODO: implement this code!

            return str.ToString();
        }

        private string GetFieldType(IDbColumnBinding column)
        {
            return MySqlDataType.Translate(column.DataType, column.FieldType);
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
                case DbKeyConstraint.None:
                    return string.Empty;

                case DbKeyConstraint.PrimaryKey:
                    return string.Empty;

                case DbKeyConstraint.UniqueKey:
                    return string.Empty;

                // {INDEX|KEY} [index_name] [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.ForeignKey:
                    return $"{KeyKey} fk_{TableName}_{column.FieldName}_idx ( {column.FieldName} )";

                default:
                    return string.Empty;
            }
        }

        private string GetColumnConstraints(IDbColumnBinding column)
        {
            switch (column.KeyConstraint)
            {
                case DbKeyConstraint.None:
                    return string.Empty;

                // [CONSTRAINT [symbol]] PRIMARY KEY [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.PrimaryKey:
                    return $"{ConstraintKey} {PrimaryKeyKey} ( {column.FieldName} )";

                // [CONSTRAINT [symbol]] UNIQUE [INDEX|KEY] [index_name] [index_type] (key_part,...) [index_option] ...
                case DbKeyConstraint.UniqueKey:
                    return $"{ConstraintKey} {UniqueKeyKey} uk_{TableName}_{column.FieldName} ( {column.FieldName} )";

                // [CONSTRAINT [symbol]] FOREIGN KEY [index_name] (col_name,...)
                // REFERENCES tbl_name (key_part,...) [MATCH FULL | MATCH PARTIAL | MATCH SIMPLE] [ON DELETE reference_option] [ON UPDATE reference_option]
                case DbKeyConstraint.ForeignKey:
                    return $"{ConstraintKey} fk_{TableName}_{column.ForeignTable} {ForeignKeyKey} ( {column.FieldName} ) {ReferecencesKey} {column.ForeignTable} ( {column.ForeignColumn} ) ON DELETE NO ACTION ON UPDATE NO ACTION";

                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
