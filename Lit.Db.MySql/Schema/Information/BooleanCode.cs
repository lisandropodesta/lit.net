using Lit.Db.Attributes;

namespace Lit.Db.MySql.Schema.Information
{
    /// <summary>
    /// Boolean code.
    /// </summary>
    public enum BooleanCode
    {
        [DbEnumCode("NO")]
        No,

        [DbEnumCode("YES")]
        Yes
    }
}
