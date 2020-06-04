using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Find record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureFind : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(UniqueKeyParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  SELECT\n" +
            "    {{@" + nameof(AllColumns) + "}}\n" +
            "  FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(UniqueKeyFilterList) + "}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureFind(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Find)
        {
        }
    }
}
