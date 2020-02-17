using Lit.Db.Attributes;

namespace Lit.Db.MySql.Schema.Information
{
    /// <summary>
    /// Table type code definitions.
    /// </summary>
    public enum TableType
    {
        Unknown,

        [DbEnumCode("BASE TABLE")]
        Table,

        [DbEnumCode("VIEW")]
        View,

        [DbEnumCode("SYSTEM VIEW")]
        SystemView
    }
}
