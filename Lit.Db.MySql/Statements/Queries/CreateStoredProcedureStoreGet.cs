using System;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Set record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureStoreGet : CreateStoredProcedureStore
    {
        public new const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  IF COALESCE( {{@filter_param}}, 0 ) = 0 THEN\n" +
            "    INSERT INTO {{@table_name}}\n" +
            "    (\n" +
            "      {{@columns}}\n" +
            "    )\n" +
            "    VALUES\n" +
            "    (\n" +
            "      {{@values}}\n" +
            "    );\n" +
            "\n" +
            "    SET {{@filter_param}} = LAST_INSERT_ID();\n" +
            "  ELSE\n" +
            "    UPDATE {{@table_name}} SET\n" +
            "      {{@columns_set}}\n" +
            "    WHERE {{@filter_field}} = {{@filter_param}};\n" +
            "  END IF;\n" +
            "\n" +
            "  SELECT\n" +
            "    {{@result_columns}}\n" +
            "  FROM {{@table_name}}\n" +
            "  WHERE {{@filter_field}} = {{@filter_param}};\n" +
            "END\n";

        [DbParameter("result_columns")]
        protected string ResultColumns { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureStoreGet(Type tableTemplate, IDbSetup setup)
            : base(tableTemplate, setup, StoredProcedureFunction.Store)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTableBinding binding, IDbColumnBinding pk)
        {
            ResultColumns = GetColumnsNames(binding, DbColumnsSelection.All);

            base.Setup(tableTemplate, dbNaming, function, binding, pk);
        }
    }
}
