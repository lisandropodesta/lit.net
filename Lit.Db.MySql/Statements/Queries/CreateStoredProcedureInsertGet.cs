using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Insert record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureInsertGet : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(NonAutoIncParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  INSERT INTO {{@" + nameof(SqlTableName) + "}}\n" +
            "  (\n" +
            "    {{@" + nameof(NonAutoIncColumns) + "}}\n" +
            "  )\n" +
            "  VALUES\n" +
            "  (\n" +
            "    {{@" + nameof(NonAutoIncParams) + "}}\n" +
            "  );\n" +
            "\n" +
            "  SELECT\n" +
            "    {{@" + nameof(AllColumns) + "}}\n" +
            "  FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(AutoIncColumn) + "}} = LAST_INSERT_ID();\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsertGet(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Insert)
        {
        }
    }
}
