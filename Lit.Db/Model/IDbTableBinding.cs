using System.Collections.Generic;

namespace Lit.Db
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
        /// Columns.
        /// </summary>
        IReadOnlyList<IDbColumnBinding> Columns { get; }

        /// <summary>
        /// Single column primary key.
        /// </summary>
        IDbColumnBinding SingleColumnPrimaryKey { get; }

        /// <summary>
        /// Single column unique key.
        /// </summary>
        IDbColumnBinding SingleColumnUniqueKey { get; }

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
    }
}
