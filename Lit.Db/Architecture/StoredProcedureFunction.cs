namespace Lit.Db.Architecture
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
        /// Single record insert and get.
        /// </summary>
        InsertGet,

        /// <summary>
        /// Single record update.
        /// </summary>
        Update,

        /// <summary>
        /// Single record update and get.
        /// </summary>
        UpdateGet,

        /// <summary>
        /// Single record insert or update.
        /// </summary>
        Set,

        /// <summary>
        /// Single record insert or update and get.
        /// </summary>
        SetGet,

        /// <summary>
        /// Single record delete.
        /// </summary>
        Delete
    }
}
