using Lit.Db.Custom.MySql;

namespace Lit.Db.MySql.Schema.Information.Tables
{
    /// <summary>
    /// Column data.
    /// </summary>
    public class ColumnData
    {
        [DbField]
        public string TableCatalog { get; protected set; }

        [DbField]
        public string TableSchema { get; protected set; }

        [DbField]
        public string TableName { get; protected set; }

        [DbField]
        public string ColumnName { get; protected set; }

        [DbField]
        public int OrdinalPosition { get; protected set; }

        [DbField]
        public string ColumnDefault { get; protected set; }

        [DbField]
        public BooleanCode IsNullable { get; protected set; }

        [DbField]
        public RawDataType DataType { get; protected set; }

        [DbField]
        public int? CharacterMaximumLength { get; protected set; }

        [DbField]
        public int? CharacterOctetLength { get; protected set; }

        [DbField]
        public int? NumericPrecision { get; protected set; }

        [DbField]
        public int? NumericScale { get; protected set; }

        [DbField]
        public int? DatetimePrecision { get; protected set; }

        [DbField]
        public string CharacterSetName { get; protected set; }

        [DbField]
        public string CollationName { get; protected set; }

        [DbField]
        public string ColumnType { get; protected set; }

        [DbField]
        public ColumnKey ColumnKey { get; protected set; }

        [DbField]
        public string Extra { get; protected set; }

        [DbField]
        public string Privileges { get; protected set; }

        [DbField]
        public string ColumnComment { get; protected set; }

        [DbField]
        public string GenerationExpression { get; protected set; }
    }
}
