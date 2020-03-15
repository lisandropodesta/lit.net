using System;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Update record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureUpdateGet : CreateStoredProcedureUpdate
    {
        public new const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  UPDATE {{@table_name}} SET\n" +
            "    {{@columns_set}}\n" +
            "  WHERE {{@filter_field}} = {{@filter_param}};\n" +
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
        public CreateStoredProcedureUpdateGet(Type tableTemplate, IDbSetup setup)
            : base(tableTemplate, setup, StoredProcedureFunction.Update)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, IDbTableBinding binding, IDbColumnBinding pk)
        {
            base.Setup(tableTemplate, dbNaming, function, binding, pk);

            ResultColumns = GetColumnsNames(binding, DbColumnsSelection.All);
        }
    }
}
