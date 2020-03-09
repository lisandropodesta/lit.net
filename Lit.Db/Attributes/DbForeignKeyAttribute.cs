using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbForeignKeyAttribute : DbColumnAttribute
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

        public DbForeignKeyAttribute(Type tableTemplate, string propertyName = null)
        {
            ForeignTableTemplate = tableTemplate;
            ForeignColumnProperty = propertyName;
        }

        public DbForeignKeyAttribute(string name, Type tableTemplate, string propertyName = null) : base(name)
        {
            ForeignTableTemplate = tableTemplate;
            ForeignColumnProperty = propertyName;
        }

        #endregion
    }
}
