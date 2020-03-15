using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableUniqueKeyAttribute : Attribute, IDbTableUniqueKeyAttribute
    {
        /// <summary>
        /// Unique key name.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// List of field names.
        /// </summary>
        public string[] FieldNames { get; private set; }

        #region Constructors

        public DbTableUniqueKeyAttribute(params string[] fieldNames)
        {
            if ((fieldNames?.Length ?? 0) < 2)
            {
                throw new ArgumentException($"[{nameof(DbTableUniqueKeyAttribute)}] needs to be used with more than one field, use [{nameof(DbUniqueKeyAttribute)}] for a single field.");
            }

            FieldNames = fieldNames;
        }

        #endregion
    }
}
