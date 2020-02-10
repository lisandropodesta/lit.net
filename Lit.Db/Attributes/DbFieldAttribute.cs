using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Stored procedure field definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        /// <summary>
        /// Name of the field.
        /// </summary>
        public string DbName => dbName;

        protected readonly string dbName;

        /// <summary>
        /// Optional field flag.
        /// </summary>
        public bool IsOptional => isOptional;

        protected readonly bool isOptional;

        #region Constructors

        public DbFieldAttribute(bool isOptional = false) : this(null, isOptional) { }

        public DbFieldAttribute(string dbName = null, bool isOptional = false)
        {
            this.dbName = dbName;
            this.isOptional = isOptional;
        }

        #endregion
    }
}
