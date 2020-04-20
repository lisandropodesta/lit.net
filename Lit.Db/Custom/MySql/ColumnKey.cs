namespace Lit.Db.Custom.MySql
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
