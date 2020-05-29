using System;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure field definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        /// <summary>
        /// Name at DB.
        /// </summary>
        public string DbName => name;

        protected readonly string name;

        /// <summary>
        /// Optional field flag.
        /// </summary>
        public bool IsOptional => isOptional;

        protected readonly bool isOptional;

        /// <summary>
        /// IsNullable forced. Used when defining a field from a column.
        /// </summary>
        public bool? IsNullableForced { get; private set; }

        #region Constructors

        public DbFieldAttribute() { }

        public DbFieldAttribute(bool isOptional = false) : this(null, isOptional) { }

        public DbFieldAttribute(string name, bool isOptional = false)
        {
            this.name = name;
            this.isOptional = isOptional;
        }

        public DbFieldAttribute(DbColumnAttribute colAttr)
        {
            name = colAttr.DbName;
            isOptional = true;
            IsNullableForced = colAttr.IsNullableForced;
        }

        #endregion
    }
}
