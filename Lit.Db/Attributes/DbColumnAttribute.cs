using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Table column definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : DbFieldAttribute
    {
        #region Constructors

        public DbColumnAttribute() : base(false) { }

        public DbColumnAttribute(string name) : base(name, false) { }

        #endregion
    }
}
