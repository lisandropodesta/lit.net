using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Table attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        /// <summary>
        /// Name of the table.
        /// </summary>
        public string TableName => name;

        private readonly string name;

        #region Constructors

        public DbTableAttribute()
        {
        }

        public DbTableAttribute(string name)
        {
            this.name = name;
        }

        #endregion
    }
}
