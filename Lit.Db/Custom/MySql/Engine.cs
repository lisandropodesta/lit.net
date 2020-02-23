using Lit.Db.Attributes;

namespace Lit.Db.Custom.MySql
{
    /// <summary>
    /// Engine codes definition.
    /// </summary>
    public enum Engine
    {
        Unknown,

        [DbEnumCode("InnoDB")]
        InnoDb,

        [DbEnumCode("MyISAM")]
        MyISAM,

        [DbEnumCode("MEMORY")]
        Memory,

        [DbEnumCode("CSV")]
        CSV,

        [DbEnumCode("ARCHIVE")]
        Archive,

        [DbEnumCode("BLACKHOLE")]
        BlacHole,

        [DbEnumCode("MRG_MYISAM")]
        Merge,

        [DbEnumCode("NDB")]
        NDB,

        [DbEnumCode("PERFORMANCE_SCHEMA")]
        PerformanceSchema,

        [DbEnumCode("FEDERATED")]
        Federated,

        [DbEnumCode("EXAMPLE")]
        Example
    }
}
