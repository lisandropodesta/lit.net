using System;
using Lit.Db.Architecture;
using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Insert record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureInsertGet : CreateStoredProcedureInsert
    {
        public new const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  INSERT INTO {{@table_name}}\n" +
            "  (\n" +
            "    {{@columns}}\n" +
            "  )\n" +
            "  VALUES\n" +
            "  (\n" +
            "    {{@values}}\n" +
            "  );\n" +
            "\n" +
            "  SELECT\n" +
            "    {{@columns}}\n" +
            "  FROM {{@table_name}}\n" +
            "  WHERE {{@filter_field}} = LAST_INSERT_ID();\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsertGet(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.InsertGet)
        {
        }
    }
}
