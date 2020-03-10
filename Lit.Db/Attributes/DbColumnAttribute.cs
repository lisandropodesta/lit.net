using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Table column definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute
    {
        /// <summary>
        /// Name at DB.
        /// </summary>
        public string DbName => name;

        protected readonly string name;

        /// <summary>
        /// IsNullable flag defined flag.
        /// </summary>
        public virtual bool IsNullableDefined => isNullable != null;

        /// <summary>
        /// IsNullable flag.
        /// </summary>
        public virtual bool IsNullable { get => isNullable ?? false; set => isNullable = value; }

        private bool? isNullable;

        #region Constructors

        public DbColumnAttribute() { }

        public DbColumnAttribute(string name)
        {
            this.name = name;
        }

        #endregion
    }
}
