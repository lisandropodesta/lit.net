namespace Lit.Db.Framework
{
    /// <summary>
    /// Pre defined stored procedure functions.
    /// </summary>
    public enum StoredProcedureFunction
    {
        /// <summary>
        /// Single record get.
        /// </summary>
        Get,

        /// <summary>
        /// Single records get by unique code.
        /// </summary>
        GetByCode,

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
        /// Single record insert or update.
        /// </summary>
        Set,

        /// <summary>
        /// Single record delete.
        /// </summary>
        Delete
    }
}
