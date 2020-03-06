using System;
using System.Data;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Get record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureGet : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  SELECT\n" +
            "    {{@columns}}\n" +
            "  FROM {{@table_name}}\n" +
            "  WHERE {{@filter_field}} = {{@filter_param}};\n" +
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureGet(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function)
            : base(tableTemplate, dbNaming, function)
        {
        }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            var filterCol = pk;

            if (function == StoredProcedureFunction.GetByCode)
            {
                filterCol = binding.FindFirstColumn(DbColumnsSelection.UniqueKey);
                if (filterCol == null)
                {
                    throw new ArgumentException($"Unable Invalid unique key in table template [{tableTemplate.FullName}]");
                }

                FilterField = filterCol.FieldName;
                FilterParam = filterCol.SpParamName;
            }

            AddParameter(filterCol, ParameterDirection.Input);

            Columns = GetColumnsNames(binding, DbColumnsSelection.All);
        }
    }
}
