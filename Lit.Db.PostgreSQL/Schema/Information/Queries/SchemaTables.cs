using System.Collections.Generic;

namespace Lit.Db.PostgreSQL.Schema.Information.Queries
{
    public class SchemaTables
    {
        [DbRecordset]
        public List<TableData> Tables { get; set; }

        public SchemaTables()
        {
            Tables = new List<TableData>();
        }
    }

    public class TableData
    {
        [DbColumn("table_name")]
        public string TableName { get; set; }

        [DbColumn("table_schema")]
        public string TableSchema { get; set; }

        [DbColumn("table_type")]
        public string TableType { get; set; }
    }
}
