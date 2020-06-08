using System.Collections.Generic;

namespace Lit.DataType
{
    /// <summary>
    /// Type binding.
    /// </summary>
    public interface ITypeBinding<TI>
    {
        /// <summary>
        /// Binding list.
        /// </summary>
        IReadOnlyList<TI> BindingList { get; }
    }
}
