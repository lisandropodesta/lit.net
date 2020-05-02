using System;

namespace Lit.Db
{
    public interface IDbForeignKeyAttribute
    {
        /// <summary>
        /// Primary table template.
        /// </summary>
        Type PrimaryTableTemplate { get; }

        /// <summary>
        /// Primary column property name.
        /// </summary>
        string PrimaryColumnProperty { get; }
    }
}
