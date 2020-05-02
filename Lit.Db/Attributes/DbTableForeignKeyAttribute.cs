using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableForeignKeyAttribute : DbTableKeyAttributeBase, IDbTableForeignKeyAttribute
    {
        /// <summary>
        /// Primary table template.
        /// </summary>
        public Type PrimaryTableTemplate { get; private set; }

        #region Constructors

        public DbTableForeignKeyAttribute(Type primaryTableTemplate, params string[] propertyNames)
            : base(propertyNames)
        {
            PrimaryTableTemplate = primaryTableTemplate;
        }

        #endregion
    }
}
