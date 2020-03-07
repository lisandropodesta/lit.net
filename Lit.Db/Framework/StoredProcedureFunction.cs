namespace Lit.Db.Framework
{
    /// <summary>
    /// Pre defined stored procedure functions.
    /// </summary>
    public enum StoredProcedureFunction
    {
        /// <summary>
        /// Single record get by primary key.
        /// </summary>
        Get,

        /// <summary>
        /// Single record find by unique key.
        /// </summary>
        Find,

        /// <summary>
        /// List all records.
        /// </summary>
        ListAll,

        /// <summary>
        /// Single record insert.
        /// </summary>
        Insert,

        /// <summary>
        /// Single record update.
        /// </summary>
        Update,

        /// <summary>
        /// Single record store.
        /// </summary>
        Store,

        /// <summary>
        /// Single record delete.
        /// </summary>
        Delete
    }
}
