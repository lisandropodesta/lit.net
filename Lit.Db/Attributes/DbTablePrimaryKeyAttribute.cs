using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTablePrimaryKeyAttribute : DbTableKeyAttributeBase, IDbTablePrimaryKeyAttribute
    {
        #region Constructors

        public DbTablePrimaryKeyAttribute(params string[] propertyNames)
            : base(propertyNames)
        {
        }

        #endregion
    }
}
