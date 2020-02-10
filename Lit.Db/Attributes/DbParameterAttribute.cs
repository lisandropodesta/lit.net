using System;
using System.Data;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Stored procedure parameter definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbParameterAttribute : Attribute
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public string DbName => dbName;

        protected readonly string dbName;

        /// <summary>
        /// Flow of information direction 
        /// </summary>
        public ParameterDirection Direction => direction;

        protected readonly ParameterDirection direction;

        /// <summary>
        /// Optional parameter flag.
        /// </summary>
        public bool IsOptional => isOptional;

        protected readonly bool isOptional;

        #region Constructors

        public DbParameterAttribute(ParameterDirection direction, bool isOptional = false) : this(null, direction, isOptional) { }

        public DbParameterAttribute(string dbName = null, ParameterDirection direction = ParameterDirection.Input, bool isOptional = false)
        {
            this.dbName = dbName;
            this.direction = direction;
            this.isOptional = isOptional;
        }

        #endregion
    }
}
