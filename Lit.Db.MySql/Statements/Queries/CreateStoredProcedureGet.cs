using System;
using System.Data;
using System.Linq;
using System.Text;
using Lit.Db.Architecture;
using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Stored procedure creation template.
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
            "  WHERE {{@primary_key}} = {{@primary_key_param}};\n" +
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get { return columns.Length == 0 ? "*" : columns.ToString(); } set { } }

        private readonly StringBuilder columns = new StringBuilder();

        [DbParameter("table_name")]
        public string TableName { get; set; }

        [DbParameter("primary_key")]
        public string PrimaryKey { get; set; }

        [DbParameter("primary_key_param")]
        public string PrimaryKeyParam { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureGet(Type tableTemplate, IDbNaming dbNaming)
        {
            var binding = DbTemplateCache.GetTable(tableTemplate, dbNaming);

            var pk = binding.Columns.FirstOrDefault(c => IsSelected(c, ParametersSelection.PrimaryKey));
            if (pk == null)
            {
                throw new Exception($"Primary key not found for template table {tableTemplate.FullName}");
            }

            TableName = binding.Text;
            Name = dbNaming.GetStoredProcedureName(TableName, StoredProcedureFunction.Get);
            PrimaryKey = pk.FieldName;
            PrimaryKeyParam = dbNaming.GetParameterName(pk.FieldName, null);

            AddParameter(pk, ParameterDirection.Input, dbNaming);
            AddColumns(columns, ParametersSelection.All, ",\n    ", binding.Columns);
        }
    }
}
