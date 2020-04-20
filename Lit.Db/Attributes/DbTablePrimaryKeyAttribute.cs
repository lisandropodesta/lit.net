using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTablePrimaryKeyAttribute : Attribute, IDbTablePrimaryKeyAttribute
    {
        /// <summary>
        /// Primary key name.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// List of field names.
        /// </summary>
        public string[] FieldNames { get; private set; }

        #region Constructors

        public DbTablePrimaryKeyAttribute(params string[] fieldNames)
        {
            if ((fieldNames?.Length ?? 0) < 2)
            {
                throw new ArgumentException($"[{nameof(DbTablePrimaryKeyAttribute)}] needs to be used with more than one field, use [{nameof(DbPrimaryKeyAttribute)}] for a single field.");
            }

            FieldNames = fieldNames;
        }

        #endregion
    }
}
