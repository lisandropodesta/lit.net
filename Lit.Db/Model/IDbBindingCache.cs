using System;
using System.Collections.Generic;

namespace Lit.Db
{
    /// <summary>
    /// Binding cache.
    /// </summary>
    public interface IDbBindingCache
    {
        /// <summary>
        /// Gets a table binding.
        /// </summary>
        IDbTableBinding GetTableBinding(Type type);

        /// <summary>
        /// Gets a command binding.
        /// </summary>
        IDbCommandBinding GetCommandBinding(Type type);

        /// <summary>
        /// Gets parameters binding.
        /// </summary>
        IReadOnlyList<IDbParameterBinding> GetParametersBinding(Type type);
    }
}
