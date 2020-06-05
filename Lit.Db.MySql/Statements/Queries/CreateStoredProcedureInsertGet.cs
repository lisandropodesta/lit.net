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
            "{{@" + nameof(ConditionalDeclareAutoIncParam) + "}}" +
            "  INSERT INTO {{@" + nameof(SqlTableName) + "}}\n" +
            "  (\n" +
            "    {{@" + nameof(NonAutoIncColumns) + "}}\n" +
            "  )\n" +
            "  VALUES\n" +
            "  (\n" +
            "    {{@" + nameof(NonAutoIncParams) + "}}\n" +
            "  );\n" +
            "  {{@" + nameof(ConditionalAssignAutoIncParam) + "}}" +
            "\n" +
            "  SELECT\n" +
            "    {{@" + nameof(AllColumns) + "}}\n" +
            "  FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(PrimaryKeyMatchCondition) + "}};\n" +
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
