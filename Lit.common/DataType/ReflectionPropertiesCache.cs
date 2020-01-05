using System;
using System.Collections.Generic;

namespace Lit.DataType
{
    public static class ReflectionPropertiesCache
    {
        private static readonly object theLock = new object();

        private static Dictionary<Type, IReflectionProperties> cache;

        public static IReflectionProperties Get(Type type)
        {
            lock (theLock)
            {
                if (cache == null)
                {
                    cache = new Dictionary<Type, IReflectionProperties>();
                }

                if (!cache.TryGetValue(type, out IReflectionProperties dict))
                {
                    dict = TypeHelper.GetPropertiesDict(type);
                    cache.Add(type, dict);
                }

                return dict;
            }
        }
    }

    public static class ReflectionPropertiesCache<T> where T : class
    {
        private static readonly object theLock = new object();

        private static Dictionary<Type, IReflectionProperties<T>> cache;

        public static IReflectionProperties<T> Get(Type type)
        {
            lock (theLock)
            {
                if (cache == null)
                {
                    cache = new Dictionary<Type, IReflectionProperties<T>>();
                }

                if (!cache.TryGetValue(type, out IReflectionProperties<T> dict))
                {
                    dict = TypeHelper.GetPropertiesDict<T>(type);
                    cache.Add(type, dict);
                }

                return dict;
            }
        }
    }
}
