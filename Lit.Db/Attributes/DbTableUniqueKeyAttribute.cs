using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableUniqueKeyAttribute : DbTableKeyAttributeBase, IDbTableUniqueKeyAttribute
    {
        #region Constructors

        public DbTableUniqueKeyAttribute(params string[] propertyNames)
            : base(propertyNames)
        {
        }

        #endregion
    }
}
