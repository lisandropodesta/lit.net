using System;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure recordset definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbRecordsetAttribute : Attribute
    {
        /// <summary>
        /// Index of the recordset in the list of recordsets returned by the sp.
        /// </summary>
        public int Index => index;

        private readonly int index;

        #region Constructors

        public DbRecordsetAttribute() : this(0) { }

        public DbRecordsetAttribute(int index)
        {
            this.index = index;
        }

        #endregion
    }
}
