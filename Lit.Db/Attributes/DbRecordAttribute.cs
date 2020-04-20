using System;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure record definition (for recordset results).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbRecordAttribute : Attribute
    {
        /// <summary>
        /// Index of the recordset in the list of recordsets returned by the sp.
        /// </summary>
        public int Index => index;

        private readonly int index;

        public bool AllowMultipleRecords => allowMultipleRecords;

        private readonly bool allowMultipleRecords;

        #region Constructors

        public DbRecordAttribute(bool allowMultipleRecords = false) : this(0, allowMultipleRecords) { }

        public DbRecordAttribute(int index, bool allowRecordset = false)
        {
            this.index = index;
            this.allowMultipleRecords = allowRecordset;
        }

        #endregion
    }
}
