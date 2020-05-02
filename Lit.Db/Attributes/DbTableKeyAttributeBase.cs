using System;

namespace Lit.Db
{
    public abstract class DbTableKeyAttributeBase : Attribute, IDbTableKeyAttribute
    {
        /// <summary>
        /// Primary key name.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// List of properties.
        /// </summary>
        public string[] PropertyNames { get; private set; }

        #region Constructor

        protected DbTableKeyAttributeBase(string[] propertyNames)
        {
            PropertyNames = propertyNames ?? new string[] { };
        }

        #endregion
    }
}
