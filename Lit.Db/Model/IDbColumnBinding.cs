using System;
using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbColumnBinding : IDbPropertyBinding<DbColumnAttribute>
    {
        /// <summary>
        /// Field name.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Field type.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Field size.
        /// </summary>
        ulong? FieldSize { get; }

        /// <summary>
        /// Nullable flag.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        /// Key constraint.
        /// </summary>
        DbKeyConstraint KeyConstraint { get; }

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        bool IsAutoIncrement { get; }

        /// <summary>
        /// Foreign table.
        /// </summary>
        string ForeignTable { get; }

        /// <summary>
        /// Foreign column.
        /// </summary>
        string ForeignColumn { get; }

        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }

        /// <summary>
        /// Assign foreign key.
        /// </summary>
        void AssignForeignKey(string foreignTable, string foreignColumn);

        /// <summary>
        /// Get output field.
        /// </summary>
        void GetOutputField(DbDataReader reader, object instance);

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);
    }
}
