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
        /// Foreign table template.
        /// </summary>
        public Type ForeignTableTemplate { get; private set; }

        /// <summary>
        /// Foreign column property name.
        /// </summary>
        public string ForeignColumnProperty { get; private set; }

        #region Constructors

        public DbPrimaryAndForeignKeyAttribute(Type tableTemplate, string propertyName = null) : base(false)
        {
            ForeignTableTemplate = tableTemplate;
            ForeignColumnProperty = propertyName;
        }

        public DbPrimaryAndForeignKeyAttribute(string name, Type tableTemplate, string propertyName = null) : base(name, false)
        {
            ForeignTableTemplate = tableTemplate;
            ForeignColumnProperty = propertyName;
        }

        #endregion
    }
}
