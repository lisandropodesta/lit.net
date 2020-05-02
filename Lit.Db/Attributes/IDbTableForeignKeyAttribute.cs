using System;

namespace Lit.Db
{
    public interface IDbTableForeignKeyAttribute : IDbTableKeyAttribute
    {
        /// <summary>
        /// Primary table template.
        /// </summary>
        Type PrimaryTableTemplate { get; }
    }
}
