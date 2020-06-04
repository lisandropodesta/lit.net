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
        /// Auto incremented.
        /// </summary>
        AutoInc,

        /// <summary>
        /// Non auto incremented.
        /// </summary>
        NonAutoInc,

        /// <summary>
        /// Primary key.
        /// </summary>
        PrimaryKey,

        /// <summary>
        /// Non primary keys.
        /// </summary>
        NonPrimaryKey,

        /// <summary>
        /// Unique key.
        /// </summary>
        UniqueKey
    }
}
