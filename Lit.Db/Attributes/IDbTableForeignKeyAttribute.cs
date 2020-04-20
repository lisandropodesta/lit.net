using System;

namespace Lit.Db
{
    public interface IDbTableForeignKeyAttribute : IDbTableKeyAttribute
    {
        /// <summary>
        /// Foreign table template.
        /// </summary>
        Type ForeignTableTemplate { get; }

        /// <summary>
        /// Foreign table.
        /// </summary>
        string ForeignTable { get; set; }

        /// <summary>
        /// Foreign columns.
        /// </summary>
        string[] ForeignColumns { get; set; }
    }
}
