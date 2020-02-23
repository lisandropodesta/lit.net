using Lit.Db.Attributes;

namespace Lit.Db.Custom.MySql
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
