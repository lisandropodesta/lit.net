﻿using System;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// Set record stored procedure creation template.
    /// </summary>
    [DbQuery(Template)]
    public class CreateStoredProcedureStoreGet : CreateStoredProcedureTemplate
    {
        public const string Template =
            "CREATE PROCEDURE {{@" + nameof(SqlSpName) + "}}({{@" + nameof(AllParamsDef) + "}})\n" +
            "BEGIN\n" +
            "  IF {{@" + nameof(RecordDoNotExistsCondition) + "}} THEN\n" +
            "    INSERT INTO {{@" + nameof(SqlTableName) + "}}\n" +
            "    (\n" +
            "      {{@" + nameof(NonAutoIncColumns) + "}}\n" +
            "    )\n" +
            "    VALUES\n" +
            "    (\n" +
            "      {{@" + nameof(NonAutoIncParams) + "}}\n" +
            "    );\n" +
            "    {{@"+ nameof(ConditionalAssignAutoIncParam) +"}}" +
            "  ELSE\n" +
            "    UPDATE {{@" + nameof(SqlTableName) + "}} SET\n" +
            "      {{@" + nameof(NonPrimaryColumsSet) + "}}\n" +
            "    WHERE {{@" + nameof(PrimaryKeyMatchCondition) + "}};\n" +
            "  END IF;\n" +
            "\n" +
            "  SELECT\n" +
            "    {{@" + nameof(AllColumns) + "}}\n" +
            "  FROM {{@" + nameof(SqlTableName) + "}}\n" +
            "  WHERE {{@" + nameof(PrimaryKeyMatchCondition) + "}};\n" +
            "END\n";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateStoredProcedureStoreGet(IDbSetup setup, Type tableTemplate)
            : base(setup, tableTemplate, StoredProcedureFunction.Store)
        {
        }
    }
}
