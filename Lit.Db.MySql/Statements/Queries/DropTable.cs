using System;

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
        public DropTable(IDbSetup setup, Type tableTemplate, bool onlyIfExists = false) : base(setup)
        {
            var bindings = setup.GetTableBinding(tableTemplate);
            TableName = bindings.TableName;
            OnlyIfExists = onlyIfExists;
        }

        private void SetOnlyIfExists(bool value)
        {
            onlyIfExists = value;
            IfExists = onlyIfExists ? IfExistsKey : string.Empty;
        }
    }
}
