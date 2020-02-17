using Lit.Db.Attributes;

namespace Lit.Db.MySql.Schema.Information
{
    /// <summary>
    /// Column key codes definition.
    /// </summary>
    public enum ColumnKey
    {
        [DbEnumCode("")]
        None,

        [DbEnumCode("PRI")]
        Primary,

        [DbEnumCode("UNI")]
        Unique,

        [DbEnumCode("MUL")]
        NonUnique
    }
}
