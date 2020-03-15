using System;
using System.Data;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Insert record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureInsert : CreateStoredProcedureTemplate
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
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get; set; }

        [DbParameter("values")]
        protected string Values { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureInsert(Type tableTemplate, IDbSetup setup, StoredProcedureFunction function)
            : base(tableTemplate, setup, function)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureInsert(Type tableTemplate, IDbSetup setup)
            : base(tableTemplate, setup, StoredProcedureFunction.Insert)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, IDbTableBinding binding, IDbColumnBinding pk)
        {
            AddParameters(binding, DbColumnsSelection.NonPrimaryKey, ParameterDirection.Input);

            Columns = GetColumnsNames(binding, DbColumnsSelection.NonPrimaryKey);

            Values = GetParametersNames(binding, DbColumnsSelection.NonPrimaryKey);
        }
    }
}
