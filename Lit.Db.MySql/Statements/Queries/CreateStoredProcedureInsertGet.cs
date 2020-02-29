using System;
using System.Data;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Insert record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureInsertGet : CreateStoredProcedureTemplate
    {
        public const string Template =
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

        [DbParameter("columns")]
        protected string Columns { get; set; }

        [DbParameter("values")]
        protected string Values { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsertGet(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.InsertGet)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            AddParameters(binding, ParametersSelection.NonPrimaryKey, ParameterDirection.Input, dbNaming);

            Columns = GetColumnsNames(binding, ParametersSelection.NonPrimaryKey);

            Values = GetParametersNames(binding, ParametersSelection.NonPrimaryKey, dbNaming);
        }
    }
}
