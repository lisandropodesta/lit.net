using System;
using System.Data;

namespace Lit.Db
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
        public string ParameterName => name;

        protected readonly string name;

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

        /// <summary>
        /// Do not translate in query replacements.
        /// </summary>
        public bool DoNotTranslate { get; set; }

        /// <summary>
        /// Is input flag.
        /// </summary>
        public bool IsInput => direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput;

        /// <summary>
        /// Is output flag.
        /// </summary>
        public bool IsOutput => direction == ParameterDirection.Output || direction == ParameterDirection.InputOutput;

        #region Constructors

        public DbParameterAttribute(ParameterDirection direction, bool isOptional = false) : this(null, direction, isOptional) { }

        public DbParameterAttribute(string name = null, ParameterDirection direction = ParameterDirection.Input, bool isOptional = false)
        {
            this.name = name;
            this.direction = direction;
            this.isOptional = isOptional;
        }

        #endregion
    }
}
