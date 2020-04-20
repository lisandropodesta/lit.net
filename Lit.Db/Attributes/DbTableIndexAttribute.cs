using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableIndexAttribute : Attribute, IDbTableIndexAttribute
    {
        /// <summary>
        /// Primary key name.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// Is unique flag.
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// List of field names.
        /// </summary>
        public string[] FieldNames { get; private set; }

        #region Constructors

        public DbTableIndexAttribute(params string[] fieldNames)
        {
            FieldNames = fieldNames;
        }

        #endregion
    }
}
