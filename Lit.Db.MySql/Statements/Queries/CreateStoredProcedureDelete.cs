using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Delete record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureDelete : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(PrimaryKeyParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  DELETE FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(PrimaryKeyFilterList) + "}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureDelete(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Delete)
        {
        }
    }
}
