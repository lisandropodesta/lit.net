namespace Lit.Db
{
    /// <summary>
    /// Base class for records in memory.
    /// </summary>
    public abstract class DbRecordBase : IDbDataAccessRef
    {
        /// <summary>
        /// Data access reference.
        /// </summary>
        public IDbDataAccess Db { get; set; }
    }
}
