using System;
using System.Collections.Generic;

namespace Lit.DataType
{
    /// <summary>
    /// Type binding cache.
    /// </summary>
    public class TypeBindingCache<TI, TA> where TI : class where TA : class
    {
        private readonly Dictionary<Type, ITypeBinding<TI>> cache = new Dictionary<Type, ITypeBinding<TI>>();

        /// <summary>
        /// Get the binding information for a type.
        /// </summary>
        public ITypeBinding<TI> Get(Type type)
        {
            lock (cache)
            {
                if (!cache.TryGetValue(type, out ITypeBinding<TI> dict))
                {
                    dict = new TypeBinding<TI, TA>(type);
                    cache.Add(type, dict);
                }

                return dict;
            }
        }
    }
}
