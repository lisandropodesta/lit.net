using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Drop stored procedure.
    /// </summary>
    [DbQuery(Template)]
    public class DropStoredProcedure : MySqlTemplate
    {
        public const string Template = "DROP PROCEDURE {{@if_exists}} {{@name}}";

        /// <summary>
        /// Stored procedure name.
        /// </summary>
        [DbParameter("name")]
        public string Name { get; set; }

        /// <summary>
        /// Only if exists flag.
        /// </summary>
        public bool OnlyIfExists { get; set; }

        /// <summary>
        /// IF EXISTS flag.
        /// </summary>
        [DbParameter("if_exists")]
        protected string IfExists { get { return OnlyIfExists ? IfExistsKey : string.Empty; } set { } }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropStoredProcedure(IDbSetup setup, Type tableTemplate, StoredProcedureFunction function, bool onlyIfExists = false) : base(setup)
        {
            var binding = setup.GetTableBinding(tableTemplate);
            Name = setup.Naming.GetStoredProcedureName(binding.TableName, function);
            OnlyIfExists = onlyIfExists;
        }
    }
}
