using System;

namespace Lit.Db
{
    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbForeignKeyAttribute : DbColumnAttribute, IDbForeignKeyAttribute
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

        public DbForeignKeyAttribute(Type tableTemplate, string propertyName = null)
        {
            PrimaryTableTemplate = tableTemplate;
            PrimaryColumnProperty = propertyName;
        }

        public DbForeignKeyAttribute(string name, Type primaryTableTemplate, string propertyName = null) : base(name)
        {
            PrimaryTableTemplate = primaryTableTemplate;
            PrimaryColumnProperty = propertyName;
        }

        #endregion
    }
}
