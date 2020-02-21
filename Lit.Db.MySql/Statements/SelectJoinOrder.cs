using Lit.Db.Attributes;

namespace Lit.Db.MySql.Statements
{
    public enum SelectJoinOrder
    {
        [DbEnumCode("")]
        Default,

        [DbEnumCode("STRAIGHT_JOIN")]
        Straight
    }
}
