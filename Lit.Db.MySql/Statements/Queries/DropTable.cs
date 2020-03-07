using System;
using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements
{
    /// <summary>
    /// Single table dropping.
    /// </summary>
    [DbQuery(Template)]
    public class DropTable : MySqlTemplate
    {
        public const string Template = "DROP TABLE {{@if_exists}} {{@table_name}}";

        /// <summary>
        /// Table name.
        /// </summary>
        [DbParameter("table_name")]
        public string TableName { get; set; }

        /// <summary>
        /// Only if exists flag.
        /// </summary>
        public bool OnlyIfExists { get => onlyIfExists; set => SetOnlyIfExists(value); }

        private bool onlyIfExists;

        /// <summary>
        /// IF EXISTS flag.
        /// </summary>
        [DbParameter("if_exists")]
        protected string IfExists { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropTable(Type tableTemplate, IDbSetup setup, bool onlyIfExists = false)
        {
            var bindings = setup.GetTableBinding(tableTemplate);
            TableName = bindings.Text;
            OnlyIfExists = onlyIfExists;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropTable(string tableName, bool onlyIfExists = false)
        {
            TableName = tableName;
            OnlyIfExists = onlyIfExists;
        }

        private void SetOnlyIfExists(bool value)
        {
            onlyIfExists = value;
            IfExists = onlyIfExists ? IfExistsKey : string.Empty;
        }
    }
}
