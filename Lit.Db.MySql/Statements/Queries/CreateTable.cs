using System;
using System.Text;
using MySql.Data.MySqlClient;
using Lit.Db.Attributes;
using Lit.Db.Model;
using Lit.Db.MySql.Schema.Information;

namespace Lit.Db.MySql.Statements.Queries
{
    [DbQuery(Template)]
    public class CreateTable
    {
        public const string Template =
            "CREATE TABLE `{{@table_name}}` (\n" +
            "  {{@column_definition}}\n" +
            "  {{@table_constraints}}\n" +
            ") ENGINE={{@engine}} DEFAULT CHARSET={{@default_charset}}";

        /// <summary>
        /// Engine.
        /// </summary>
        [DbParameter]
        public string Engine { get; set; }

        /// <summary>
        /// Table name
        /// </summary>
        [DbParameter]
        public string TableName { get; set; }

        /// <summary>
        /// Column definition
        /// </summary>
        [DbParameter]
        public string ColumnDefinition { get; set; }

        /// <summary>
        /// Table constraints.
        /// </summary>
        [DbParameter(isOptional: true)]
        public string TableConstraints { get; set; }

        /// <summary>
        /// Default charset.
        /// </summary>
        [DbParameter]
        public string DefaultCharset { get; set; }

        /// <summary>
        /// Statemente execution.
        /// </summary>
        public CreateTable(IDbCommands db, Engine engine, string tableName, string defaultCharset, Type tableTemplate)
        {
            Engine = engine.ToString();
            TableName = tableName;
            DefaultCharset = defaultCharset;

            var bindings = DbTemplateBinding<MySqlCommand>.Get(tableTemplate, db.DbNaming);
            ColumnDefinition = GetColumnDefinition(tableTemplate, bindings);
            TableConstraints = GetTableConstraints(tableTemplate, bindings);

            db.ExecuteTemplate(this);
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
            // TODO: implement this code!
            return field.FieldType.Name;
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
