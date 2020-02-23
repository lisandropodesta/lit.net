using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public abstract class CreateStoredProcedure : CreateStoredProcedureTemplate
    {
        public const string Template = "CREATE PROCEDURE {{@name}} {{@parameters}} {{@routine_body}}";
    }
}
