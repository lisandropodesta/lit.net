namespace Lit.Db
{
    /// <summary>
    /// Columns selection.
    /// </summary>
    public enum DbColumnsSelection
    {
        /// <summary>
        /// No column.
        /// </summary>
        None,

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
