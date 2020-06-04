using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Set record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureStore : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(AllParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  IF COALESCE( {{@" + nameof(AutoIncParam) + "}}, 0 ) = 0 THEN\n" +
            "    INSERT INTO {{@" + nameof(SqlTableName) + "}}\n" +
            "    (\n" +
            "      {{@" + nameof(NonAutoIncColumns) + "}}\n" +
            "    )\n" +
            "    VALUES\n" +
            "    (\n" +
            "      {{@" + nameof(NonAutoIncParams) + "}}\n" +
            "    );\n" +
            "  ELSE\n" +
            "    UPDATE {{@" + nameof(SqlTableName) + "}} SET\n" +
            "      {{@" + nameof(NonPrimaryColumsSet) + "}}\n" +
            "    WHERE {{@" + nameof(PrimaryKeyFilterList) + "}};\n" +
            "  END IF;\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureStore(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Store)
        {
        }
    }
}
