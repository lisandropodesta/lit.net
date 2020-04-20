namespace Lit.Db.Test.Schema
{
    /// <summary>
    /// General status.
    /// </summary>
    public enum Status
    {
        [DbEnumCode("Deleted")]
        Deleted,

        [DbEnumCode("Hold")]
        Hold,

        [DbEnumCode("Active")]
        Active
    }
}
