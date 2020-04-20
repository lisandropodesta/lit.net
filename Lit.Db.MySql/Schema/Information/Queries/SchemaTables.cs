using System.Collections.Generic;
using Lit.Db.MySql.Schema.Information.Tables;
using Lit.Db.MySql.Statements;

namespace Lit.Db.MySql.Schema.Information.Queries
{
    public class SchemaTables : Select
    {
        [DbRecordset]
        public List<TableData> Tables { get; protected set; }

        public SchemaTables(string schema)
            : base("INFORMATION_SCHEMA.TABLES", $"table_schema = '{schema}'")
        {
        }
    }
}
