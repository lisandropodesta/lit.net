using System;
using System.Text;
using MySql.Data.MySqlClient;
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
            "  {{@column_definition}}\n" +
            "  {{@table_constraints}}\n" +
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
        /// Statemente execution.
        /// </summary>
        public CreateTable(Engine engine, string tableName, string defaultCharset, Type tableTemplate, IDbNaming dbNaming)
        {
            Engine = engine.ToString();
            TableName = tableName;
            DefaultCharset = defaultCharset;

            var bindings = DbTemplateBinding<MySqlCommand>.Get(tableTemplate, dbNaming);
            ColumnDefinition = GetColumnDefinition(tableTemplate, bindings);
            TableConstraints = GetTableConstraints(tableTemplate, bindings);
        }

        private string GetColumnDefinition(Type tableTemplate, DbTemplateBinding<MySqlCommand> bindings)
        {
            var str = new StringBuilder();

            var first = true;
            foreach (var field in bindings.Fields)
            {
                if (!first)
                {
                    str.Append(",\n");
                }

                str.Append($"`{field.FieldName}` {GetFieldType(field)} {GetNullable(field)} {GetDefault(field)}");

                first = false;
            }

            return str.ToString();
        }

        private string GetTableConstraints(Type tableTemplate, DbTemplateBinding<MySqlCommand> bindings)
        {
            var str = new StringBuilder();

            // TODO: implement this code!

            return str.ToString();
        }

        private string GetFieldType(IDbFieldBinding field)
        {
            return MySqlDataType.Translate(field.DataType);
        }

        private string GetNullable(IDbFieldBinding field)
        {
            return field.IsNullable ? "DEFAULT NULL" : "NOT NULL";
        }

        private object GetDefault(IDbFieldBinding field)
        {
            // TODO: implement this code!
            return string.Empty;
        }
    }
}
