using System;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Db property binding.
    /// </summary>
    public interface IDbPropertyBinding<TA> : IPropertyBinding
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Database data type.
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// Attributes.
        /// </summary>
        TA Attributes { get; }

        /// <summary>
        /// Foreign key property flag.
        /// </summary>
        bool IsForeignKeyProp { get; }

        /// <summary>
        /// Primary table template (when this property is a foreign key).
        /// </summary>
        Type PrimaryTableTemplate { get; }
    }
}
