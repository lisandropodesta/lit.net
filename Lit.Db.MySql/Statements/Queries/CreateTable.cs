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

        public const string PrimaryKeyKey = "PRIMARY KEY";

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

            var first = true;
            foreach (var col in columns)
            {
                if (!first)
                {
                    str.Append(",\n");
                }

                str.Append($"  `{col.FieldName}`");

                AddText(str, GetFieldType(col));
                AddText(str, GetNullable(col));
                AddText(str, GetDefault(col));
                AddText(str, GetAutoIncrement(col));
                AddText(str, GetFieldConstraints(col));
                AddText(str, GetReferenceDefinition(col));

                first = false;
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
            return MySqlDataType.Translate(column.DataType);
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

        private string GetFieldConstraints(IDbColumnBinding column)
        {
            switch (column.KeyConstraint)
            {
                case DbKeyConstraint.None:
                default:
                    return string.Empty;

                case DbKeyConstraint.PrimaryKey:
                    return PrimaryKeyKey;

                case DbKeyConstraint.UniqueKey:
                    return UniqueKeyKey;

                case DbKeyConstraint.ForeignKey:
                    return string.Empty;
            }
        }

        private string GetReferenceDefinition(IDbColumnBinding column)
        {
            if (column.KeyConstraint == DbKeyConstraint.ForeignKey)
            {
                return $"{ReferecencesKey} {column.ForeignTable} ( {column.ForeignColumn} )";
            }

            return string.Empty;
        }
        
        #endregion
    }
}
