using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Stored procedure field definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        /// <summary>
        /// Name of the field.
        /// </summary>
        public string FieldName => name;

        protected readonly string name;

        /// <summary>
        /// Optional field flag.
        /// </summary>
        public bool IsOptional => isOptional;

        protected readonly bool isOptional;

        /// <summary>
        /// IsNullable flag defined flag.
        /// </summary>
        public bool IsNullableDefined => isNullable != null;

        /// <summary>
        /// IsNullable flag.
        /// </summary>
        public bool IsNullable { get => isNullable ?? false; set => isNullable = value; }

        private bool? isNullable;

        #region Constructors

        public DbFieldAttribute() { }

        public DbFieldAttribute(bool isOptional = false) : this(null, isOptional) { }

        public DbFieldAttribute(string name, bool isOptional = false)
        {
            this.name = name;
            this.isOptional = isOptional;
        }

        #endregion
    }
}
