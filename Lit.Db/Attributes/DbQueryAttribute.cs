using System;

namespace Lit.Db
{
    /// <summary>
    /// Query attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbQueryAttribute : Attribute
    {
        /// <summary>
        /// Name of the stored procedure.
        /// </summary>
        public string QueryText => text;

        private readonly string text;

        #region Constructors

        public DbQueryAttribute()
        {
        }

        public DbQueryAttribute(string text)
        {
            this.text = text;
        }

        #endregion
    }
}
