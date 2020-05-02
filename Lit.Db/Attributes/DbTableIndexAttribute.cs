using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableIndexAttribute : DbTableKeyAttributeBase, IDbTableIndexAttribute
    {
        /// <summary>
        /// Is unique flag.
        /// </summary>
        public bool IsUnique { get; set; }

        #region Constructors

        public DbTableIndexAttribute(params string[] propertyNames)
            : base(propertyNames)
        {
        }

        #endregion
    }
}
