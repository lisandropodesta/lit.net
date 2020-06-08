using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// Type binding cache.
    /// </summary>
    public class TypeBindingCache<TA> : TypeBindingCache<IAttrPropertyBinding<TA>, TA> where TA : class
    {
        /// <summary>
        /// Get the binding information for a type.
        /// </summary>
        public ITypeBinding<IAttrPropertyBinding<TA>> Get(Type type)
        {
            return Get(type, () => new TypeBinding<TA>(type));
        }
    }

    /// <summary>
    /// Type binding cache.
    /// </summary>
    public class TypeBindingCache<TI, TA> where TI : class where TA : class
    {
        protected readonly Dictionary<Type, ITypeBinding<TI>> cache = new Dictionary<Type, ITypeBinding<TI>>();

        /// <summary>
        /// Get the binding information for a type.
        /// </summary>
        public ITypeBinding<TI> Get(Type type, Func<PropertyInfo, TA, TI> createInstace)
        {
            return Get(type, () => new TypeBinding<TI, TA>(type, createInstace));
        }

        /// <summary>
        /// Get the binding information for a type.
        /// </summary>
        protected ITypeBinding<TI> Get(Type type, Func<ITypeBinding<TI>> createBinding)
        {
            lock (cache)
            {
                if (!cache.TryGetValue(type, out ITypeBinding<TI> dict))
                {
                    dict = createBinding();
                    cache.Add(type, dict);
                }

                return dict;
            }
        }
    }
}
