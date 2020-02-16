using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Stored procedure attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbStoredProcedureAttribute : Attribute
    {
        /// <summary>
        /// Name of the stored procedure.
        /// </summary>
        public string StoredProcedureName => name;

        private readonly string name;

        #region Constructors

        public DbStoredProcedureAttribute(string name)
        {
            this.name = name;
        }

        #endregion
    }
}
