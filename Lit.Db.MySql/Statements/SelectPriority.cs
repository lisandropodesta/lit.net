using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements
{
    public enum SelectPriority
    {
        [DbEnumCode("")]
        Default,

        [DbEnumCode("HIGH_PRIORITY")]
        High
    }
}
