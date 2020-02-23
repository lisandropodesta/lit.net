using Lit.Db.Attributes;

namespace Lit.Db.Custom.MySql
{
    /// <summary>
    /// Data type codes definition.
    /// </summary>
    public enum RawDataType
    {
        [DbEnumCode("bit")]
        Bit,

        [DbEnumCode("tinyint")]
        TinyInt,

        [DbEnumCode("smallint")]
        SmallInt,

        [DbEnumCode("mediumint")]
        MediumInt,

        [DbEnumCode("int")]
        Int,

        [DbEnumCode("bigint")]
        BigInt,

        [DbEnumCode("float")]
        Float,

        [DbEnumCode("real")]
        Real,

        [DbEnumCode("double")]
        Double,

        [DbEnumCode("decimal")]
        Decimal,

        [DbEnumCode("date")]
        Date,

        [DbEnumCode("time")]
        Time,

        [DbEnumCode("timestamp")]
        Timestamp,

        [DbEnumCode("year")]
        Year,

        [DbEnumCode("datetime")]
        DateTime,

        [DbEnumCode("set")]
        Set,

        [DbEnumCode("enum")]
        Enum,

        [DbEnumCode("char")]
        Char,

        [DbEnumCode("varchar")]
        Varchar,

        [DbEnumCode("text")]
        Text,

        [DbEnumCode("mediumtext")]
        MediumText,

        [DbEnumCode("longtext")]
        LongText,

        [DbEnumCode("json")]
        Json,

        [DbEnumCode("tinyblob")]
        TinyBlob,

        [DbEnumCode("blob")]
        Blob,

        [DbEnumCode("mediumblob")]
        MediumBlob,

        [DbEnumCode("longblob")]
        LongBlob
    }
}
