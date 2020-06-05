using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Update record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureUpdate : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(AllParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  UPDATE {{@" + nameof(SqlTableName) + "}} SET\n" +
            "    {{@" + nameof(NonPrimaryColumsSet) + "}}\n" +
            "  WHERE {{@" + nameof(PrimaryKeyMatchCondition) + "}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureUpdate(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Update)
        {
        }
    }
}
