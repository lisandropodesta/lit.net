using System;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Db property binding.
    /// </summary>
    public interface IDbPropertyBinding<TA> : IAttrPropertyBinding<TA>
    {
        /// <summary>
        /// Database setup.
        /// </summary>
        IDbSetup Setup { get; }

        /// <summary>
        /// Database data type.
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// Foreign key property flag.
        /// </summary>
        bool IsForeignKeyProp { get; }

        /// <summary>
        /// Key constraint.
        /// </summary>
        DbKeyConstraint KeyConstraint { get; }

        /// <summary>
        /// Primary table template (when this property is a foreign key).
        /// </summary>
        Type PrimaryTableTemplate { get; }
    }
}
