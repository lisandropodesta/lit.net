using Lit.Db.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Table template information.
    /// </summary>
    public interface IDbTableBinding : IDbTemplateBinding
    {
        /// <summary>
        /// Table name.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Resolve foreign keys.
        /// </summary>
        void ResolveForeignKeys();

        /// <summary>
        /// Columns.
        /// </summary>
        IReadOnlyList<IDbColumnBinding> Columns { get; }

        /// <summary>
        /// Primary key.
        /// </summary>
        IDbTablePrimaryKeyAttribute PrimaryKey { get; }

        /// <summary>
        /// Foreign keys.
        /// </summary>
        IReadOnlyList<IDbTableForeignKeyAttribute> ForeignKeys { get; }

        /// <summary>
        /// Unique keys.
        /// </summary>
        IReadOnlyList<IDbTableUniqueKeyAttribute> UniqueKeys { get; }

        /// <summary>
        /// Indexes.
        /// </summary>
        IReadOnlyList<IDbTableIndexAttribute> Indexes { get; }

        /// <summary>
        /// Finds a column binding.
        /// </summary>
        IDbColumnBinding FindColumn(string propertyName);

        /// <summary>
        /// Get output fields.
        /// </summary>
        void GetOutputFields(DbDataReader reader, object instance);

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance);

        /// <summary>
        /// Get the first column that matches with selection.
        /// </summary>
        IDbColumnBinding FindFirstColumn(DbColumnsSelection selection);

        /// <summary>
        /// Maps an action to every column.
        /// </summary>
        void MapColumns(DbColumnsSelection selection, Action<IDbColumnBinding> action);
    }
}
