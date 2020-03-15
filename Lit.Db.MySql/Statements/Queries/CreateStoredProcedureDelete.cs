using System;
using System.Data;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Delete record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureDelete : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@name}} {{@parameters}}\n" +
            "BEGIN\n" +
            "  DELETE FROM {{@table_name}}\n" +
            "  WHERE {{@filter_field}} = {{@filter_param}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureDelete(Type tableTemplate, IDbSetup setup)
            : base(tableTemplate, setup, StoredProcedureFunction.Delete) { }

        protected override void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, IDbTableBinding binding, IDbColumnBinding pk)
        {
            AddParameter(pk, ParameterDirection.Input);
        }
    }
}
