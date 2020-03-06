using System;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

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
            "    {{@result_columns}}\n" +
            "  FROM {{@table_name}}\n" +
            "  WHERE {{@filter_field}} = LAST_INSERT_ID();\n" +
            "END\n";

        [DbParameter("result_columns")]
        protected string ResultColumns { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsertGet(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.InsertGet)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            base.Setup(tableTemplate, dbNaming, function, binding, pk);

            ResultColumns = GetColumnsNames(binding, DbColumnsSelection.All);
        }
    }
}
