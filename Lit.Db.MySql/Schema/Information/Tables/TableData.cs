using Lit.Db.Custom.MySql;

namespace Lit.Db.MySql.Schema.Information.Tables
{
    /// <summary>
    /// Table data.
    /// </summary>
    public class TableData
    {
        [DbField]
        public string TableCatalog { get; protected set; }

        [DbField]
        public string TableSchema { get; protected set; }

        [DbField]
        public string TableName { get; protected set; }

        [DbField]
        public TableType TableType { get; protected set; }

        [DbField]
        public Engine Engine { get; protected set; }

        [DbField]
        public int Version { get; protected set; }

        [DbField]
        public RowFormat RowFormat { get; protected set; }

        [DbField]
        public int TableRows { get; protected set; }

        [DbField]
        public int? AutoIncrement { get; protected set; }

        [DbField]
        public string TableCollation { get; protected set; }

        [DbField]
        public string TableComment { get; protected set; }
    }
}
