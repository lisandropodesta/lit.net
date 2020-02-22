using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbPrimaryKeyAttribute : DbColumnAttribute
    {
        public bool AutoIncrement { get; set; }

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
