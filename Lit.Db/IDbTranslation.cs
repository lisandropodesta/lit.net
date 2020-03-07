using System;
using Lit.DataType;
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
        object ToDb<T>(BindingMode Mode, Type dstType, T value);

        /// <summary>
        /// Translates a DB value into a C# value.
        /// </summary>
        T FromDb<T>(BindingMode Mode, Type srcType, object value);
    }
}
