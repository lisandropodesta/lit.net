using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Insert record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureInsert : CreateStoredProcedureTemplate
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
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureInsert(IDbSetup setup, Type tableTemplate, StoredProcedureFunction function)
            : base(setup, tableTemplate, function)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsert(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Insert)
        {
        }
    }
}
