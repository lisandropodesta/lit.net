using System;
using System.Data;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
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
            "  WHERE {{@primary_key}} = {{@primary_key_param}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureDelete(Type tableTemplate, IDbNaming dbNaming) : base(tableTemplate, dbNaming, StoredProcedureFunction.Delete) { }

        protected override void Setup(IDbNaming dbNaming, DbTemplateBinding binding, IDbColumnBinding pk)
        {
            AddParameter(pk, ParameterDirection.Input, dbNaming);
        }
    }
}
