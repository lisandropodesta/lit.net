using System;
using System.Data;
using Lit.Db.Attributes;
using Lit.Db.Framework;
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
        protected CreateStoredProcedureUpdate(Type tableTemplate, IDbSetup setup, StoredProcedureFunction function)
            : base(tableTemplate, setup, function)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureUpdate(Type tableTemplate, IDbSetup setup)
            : base(tableTemplate, setup, StoredProcedureFunction.Update)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, IDbTableBinding binding, IDbColumnBinding pk)
        {
            AddParameters(binding, DbColumnsSelection.All, ParameterDirection.Input);

            ColumnsSet = GetFieldsAssignment(binding, DbColumnsSelection.NonPrimaryKey);
        }
    }
}
