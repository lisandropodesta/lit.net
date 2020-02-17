using System.Collections.Generic;
using Lit.Db.Attributes;
using Lit.Db.MySql.Schema.Information.Tables;

namespace Lit.Db.MySql.Schema.Information.Queries
{
    [DbQuery("select * from INFORMATION_SCHEMA.TABLES where table_schema = '{{@schema}}'")]
    public class SchemaTables
    {
        [DbParameter]
        public string Schema { get; set; }

        [DbRecordset]
        public List<TableData> Tables { get; protected set; }

        public SchemaTables() { }

        public SchemaTables(IDbCommands db, string schema)
        {
            Schema = schema;
            db.ExecuteTemplate(this);
        }
    }
}
