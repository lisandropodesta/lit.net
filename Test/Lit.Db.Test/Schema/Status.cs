using Lit.Db.Attributes;

namespace Lit.Db.Test.Schema
{
    /// <summary>
    /// General status.
    /// </summary>
    public enum Status
    {
        [DbEnumCode("Deleted")]
        Deleted,

        [DbEnumCode("Active")]
        Active
    }
}
