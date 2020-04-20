using System;

namespace Lit.Db
{
    /// <summary>
    /// Table primary key definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableForeignKeyAttribute : Attribute, IDbTableForeignKeyAttribute
    {
        /// <summary>
        /// Primary key name.
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// Foreign table template.
        /// </summary>
        public Type ForeignTableTemplate { get; private set; }

        /// <summary>
        /// List of field names.
        /// </summary>
        public string[] FieldNames { get; private set; }

        /// <summary>
        /// Foreign table.
        /// </summary>
        string IDbTableForeignKeyAttribute.ForeignTable { get; set; }

        /// <summary>
        /// Foreign column.
        /// </summary>
        string[] IDbTableForeignKeyAttribute.ForeignColumns { get; set; }

        #region Constructors

        public DbTableForeignKeyAttribute(Type foreignTableTemplate, params string[] fieldNames)
        {
            if ((fieldNames?.Length ?? 0) < 2)
            {
                throw new ArgumentException($"[{nameof(DbTableForeignKeyAttribute)}] needs to be used with more than one field, use [{nameof(DbForeignKeyAttribute)}] for a single field.");
            }

            ForeignTableTemplate = foreignTableTemplate;
            FieldNames = fieldNames;
        }

        #endregion
    }
}
