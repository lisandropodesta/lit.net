﻿using System;
using System.Data;
using System.Text;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
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
            "  WHERE {{@primary_key}} = {{@primary_key_param}};\n" +
            "END\n";

        [DbParameter("columns")]
        protected string Columns { get { return columns.Length == 0 ? "*" : columns.ToString(); } set { } }

        private readonly StringBuilder columns = new StringBuilder();

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureGet(Type tableTemplate, IDbNaming dbNaming) : base(tableTemplate, dbNaming, StoredProcedureFunction.Get) { }

        protected override void Setup(IDbNaming dbNaming, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            AddParameter(pk, ParameterDirection.Input, dbNaming);
            AddColumns(columns, ParametersSelection.All, ",\n    ", binding.Columns);
        }
    }
}
