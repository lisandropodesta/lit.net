using System;
using Lit.Db.Model;

namespace Lit.Db
{
    /// <summary>
    /// Values translation between DB and C#.
    /// </summary>
    public interface IDbTranslation
    {
        /// <summary>
        /// Translates a C# value into a DB value.
        /// </summary>
        object ToDb(DbDataType dataType, Type type, object value);

        /// <summary>
        /// Translates a DB value into a C# value.
        /// </summary>
        object FromDb(DbDataType dataType, Type type, object value);
    }
}
