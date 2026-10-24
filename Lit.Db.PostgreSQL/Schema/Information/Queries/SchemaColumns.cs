using System.Collections.Generic;

namespace Lit.Db.PostgreSQL.Schema.Information.Queries
{
    public class SchemaColumns
    {
        [DbRecordset]
        public List<ColumnData> Columns { get; set; }

        public SchemaColumns()
        {
            Columns = new List<ColumnData>();
        }
    }

    public class ColumnData
    {
        [DbColumn("column_name")]
        public string ColumnName { get; set; }

        [DbColumn("ordinal_position")]
        public int OrdinalPosition { get; set; }

        [DbColumn("column_default")]
        public string ColumnDefault { get; set; }

        [DbColumn("is_nullable")]
        public string IsNullable { get; set; }

        [DbColumn("data_type")]
        public string DataType { get; set; }

        [DbColumn("character_maximum_length")]
        public int? CharacterMaximumLength { get; set; }

        [DbColumn("numeric_precision")]
        public int? NumericPrecision { get; set; }

        [DbColumn("numeric_scale")]
        public int? NumericScale { get; set; }
    }
}
