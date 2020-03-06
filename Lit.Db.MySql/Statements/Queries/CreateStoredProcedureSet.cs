using System;
using System.Data;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Set record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureSet : CreateStoredProcedureTemplate
    {
        public const string Template =
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
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get; set; }

        [DbParameter("values")]
        protected string Values { get; set; }

        [DbParameter("columns_set")]
        protected string ColumnsSet { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureSet(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function)
            : base(tableTemplate, dbNaming, function)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureSet(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.Set)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            AddParameters(binding, DbColumnsSelection.All, ParameterDirection.Input, dbNaming);

            Indent();

            Columns = GetColumnsNames(binding, DbColumnsSelection.NonPrimaryKey);

            Values = GetParametersNames(binding, DbColumnsSelection.NonPrimaryKey, dbNaming);

            ColumnsSet = GetFieldsAssignment(binding, DbColumnsSelection.NonPrimaryKey, dbNaming);
        }
    }
}
