using System;
using System.Data;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Update record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureUpdate : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  UPDATE {{@table_name}} SET\n" +
            "    {{@columns_set}}\n" +
            "  WHERE {{@filter_field}} = {{@filter_param}};\n" +
            "END\n";

        [DbParameter("columns_set")]
        protected string ColumnsSet { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureUpdate(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.Update)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            AddParameters(binding, ParametersSelection.All, ParameterDirection.Input, dbNaming);

            ColumnsSet = GetFieldsAssignment(binding, ParametersSelection.NonPrimaryKey, dbNaming);
        }
    }
}
