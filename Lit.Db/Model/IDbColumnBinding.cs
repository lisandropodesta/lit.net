using System;

namespace Lit.Db
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbColumnBinding : IDbPropertyBinding<DbColumnAttribute>
    {
        /// <summary>
        /// Column name.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Column type.
        /// </summary>
        Type ColumnType { get; }

        /// <summary>
        /// Column size.
        /// </summary>
        ulong? ColumnSize { get; }

        /// <summary>
        /// Key constraint.
        /// </summary>
        DbKeyConstraint KeyConstraint { get; }

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        bool IsAutoIncrement { get; }

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }
    }
}
