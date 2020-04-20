using System;

namespace Lit.Db
{
    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbPrimaryKeyAttribute : DbColumnAttribute
    {
        /// <summary>
        /// IsNullable flag defined flag.
        /// </summary>
        public override bool IsNullableDefined => true;

        /// <summary>
        /// IsNullable flag.
        /// </summary>
        public override bool IsNullable => false;

        #region Constructors

        public DbPrimaryKeyAttribute()
        {
            AutoIncrement = true;
        }

        public DbPrimaryKeyAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }

        public DbPrimaryKeyAttribute(string name, bool autoIncrement) : base(name)
        {
            AutoIncrement = autoIncrement;
        }

        #endregion
    }
}
