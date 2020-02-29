using System;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Get record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureListAll : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  SELECT\n" +
            "    {{@columns}}\n" +
            "  FROM {{@table_name}};\n" +
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureListAll(Type tableTemplate, IDbNaming dbNaming)
            : base(tableTemplate, dbNaming, StoredProcedureFunction.ListAll)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            Columns = GetColumnsNames(binding, ParametersSelection.All);
        }
    }
}