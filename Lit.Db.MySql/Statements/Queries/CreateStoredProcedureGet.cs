using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Get record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureGet : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(PrimaryKeyParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  SELECT\n" +
            "    {{@" + nameof(AllColumns) + "}}\n" +
            "  FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(PrimaryKeyMatchCondition) + "}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureGet(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Get)
        {
        }
    }
}
