using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbPrimaryKeyAttribute : DbFieldAttribute
    {
        public bool AutoIncrement { get; set; }

        #region Constructors

        public DbPrimaryKeyAttribute() : base(false)
        {
            AutoIncrement = true;
        }

        public DbPrimaryKeyAttribute(bool autoIncrement) : base(false)
        {
            AutoIncrement = autoIncrement;
        }

        public DbPrimaryKeyAttribute(string name, bool autoIncrement) : base(name, false)
        {
            AutoIncrement = autoIncrement;
        }

        #endregion
    }
}
