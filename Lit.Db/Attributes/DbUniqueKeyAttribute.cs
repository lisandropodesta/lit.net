using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Unique key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbUniqueKeyAttribute : DbColumnAttribute
    {
        #region Constructors

        public DbUniqueKeyAttribute() { }

        public DbUniqueKeyAttribute(string name) : base(name) { }

        #endregion
    }
}
