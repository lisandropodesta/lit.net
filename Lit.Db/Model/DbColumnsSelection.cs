namespace Lit.Db.Model
{
    /// <summary>
    /// Columns selection.
    /// </summary>
    public enum DbColumnsSelection
    {
        /// <summary>
        /// All columns.
        /// </summary>
        All,

        /// <summary>
        /// Primary key.
        /// </summary>
        PrimaryKey,

        /// <summary>
        /// Unique key.
        /// </summary>
        UniqueKey,

        /// <summary>
        /// Non primary keys.
        /// </summary>
        NonPrimaryKey
    }
}
