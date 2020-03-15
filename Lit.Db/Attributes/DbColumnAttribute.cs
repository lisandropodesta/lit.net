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
        /// Size at DB.
        /// </summary>
        public ulong? Size { get; private set; }

        /// <summary>
        /// IsNullable flag defined flag.
        /// </summary>
        public virtual bool IsNullableDefined => isNullable != null;

        /// <summary>
        /// IsNullable flag.
        /// </summary>
        public virtual bool IsNullable { get => isNullable ?? false; set => isNullable = value; }

        private bool? isNullable;

        /// <summary>
        /// AutoIncrement flag.
        /// </summary>
        public bool AutoIncrement { get; set; }

        #region Constructors

        public DbColumnAttribute() { }

        public DbColumnAttribute(ulong size)
        {
            Size = size;
        }

        public DbColumnAttribute(string name)
        {
            this.name = name;
        }

        public DbColumnAttribute(string name, ulong size)
        {
            this.name = name;
            Size = size;
        }

        #endregion
    }
}
