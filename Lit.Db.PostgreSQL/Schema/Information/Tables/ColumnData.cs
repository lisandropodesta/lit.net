namespace Lit.Db.PostgreSQL.Schema.Information.Tables
{
    public class ColumnData
    {
        public string ColumnName { get; set; }

        public int OrdinalPosition { get; set; }

        public string ColumnDefault { get; set; }

        public string IsNullable { get; set; }

        public string DataType { get; set; }

        public int? CharacterMaximumLength { get; set; }

        public int? NumericPrecision { get; set; }

        public int? NumericScale { get; set; }
    }
}
