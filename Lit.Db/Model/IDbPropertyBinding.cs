using System.Reflection;
using Lit.DataType;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db property binding.
    /// </summary>
    public interface IDbPropertyBinding<TA> : IPropertyBinding
    {
        /// <summary>
        /// Property information.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Database data type.
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// Attributes.
        /// </summary>
        TA Attributes { get; }
    }
}
