using System;
using Lit.Db.Architecture;
using Lit.Db.Attributes;

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
        public DropStoredProcedure(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, bool onlyIfExists = false)
        {
            var binding = DbTemplateCache.GetTable(tableTemplate, dbNaming);
            Name = dbNaming.GetStoredProcedureName(binding.Text, function);
            OnlyIfExists = onlyIfExists;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropStoredProcedure(string name, bool onlyIfExists = false)
        {
            Name = name;
            OnlyIfExists = onlyIfExists;
        }
    }
}
