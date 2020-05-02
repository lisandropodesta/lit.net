using System;

namespace Lit.Db
{
    /// <summary>
    /// Simultaneous primary and foreign key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbPrimaryAndForeignKeyAttribute : DbPrimaryKeyAttribute, IDbForeignKeyAttribute
    {
        /// <summary>
        /// Primary table template.
        /// </summary>
        public Type PrimaryTableTemplate { get; private set; }

        /// <summary>
        /// Primary column property name.
        /// </summary>
        public string PrimaryColumnProperty { get; private set; }

        #region Constructors

        public DbPrimaryAndForeignKeyAttribute(Type tableTemplate, string propertyName = null) : base(false)
        {
            PrimaryTableTemplate = tableTemplate;
            PrimaryColumnProperty = propertyName;
        }

        public DbPrimaryAndForeignKeyAttribute(string name, Type tableTemplate, string propertyName = null) : base(name, false)
        {
            PrimaryTableTemplate = tableTemplate;
            PrimaryColumnProperty = propertyName;
        }

        #endregion
    }
}
