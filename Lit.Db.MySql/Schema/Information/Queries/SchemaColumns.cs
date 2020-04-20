using System.Collections.Generic;
using Lit.Db.MySql.Schema.Information.Tables;
using Lit.Db.MySql.Statements;

namespace Lit.Db.MySql.Schema.Information.Queries
{
    public class SchemaColumns : Select
    {
        [DbRecordset]
        public List<ColumnData> ColumnsList { get; protected set; }

        public SchemaColumns(string schema)
            : base("INFORMATION_SCHEMA.COLUMNS", $"table_schema = '{schema}'")
        {
        }
    }
}
