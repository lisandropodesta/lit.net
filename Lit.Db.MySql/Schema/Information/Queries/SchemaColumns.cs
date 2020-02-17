using System.Collections.Generic;
using Lit.Db.Attributes;
using Lit.Db.MySql.Schema.Information.Tables;

namespace Lit.Db.MySql.Schema.Information.Queries
{
    [DbQuery("select * from INFORMATION_SCHEMA.COLUMNS where table_schema = '{{@schema}}'")]
    public class SchemaColumns
    {
        [DbParameter]
        public string Schema { get; set; }

        [DbRecordset]
        public List<ColumnData> Columns { get; protected set; }

        public SchemaColumns() { }

        public SchemaColumns(IDbCommands db, string schema)
        {
            Schema = schema;
            db.ExecuteTemplate(this);
        }
    }
}
