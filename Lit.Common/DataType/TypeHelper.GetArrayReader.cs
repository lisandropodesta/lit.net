using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lit.DataType
{
    /// <summary>
    /// Access to a generic n dimensional array.
    /// </summary>
    public static partial class TypeHelper
    {
        // Array getters cache.
        private static readonly Dictionary<Type, Func<object, int[], object>> arrayGetters = new Dictionary<Type, Func<object, int[], object>>();

        // Array setters cache.
        private static readonly Dictionary<Type, Action<object, int[], object>> arraySetters = new Dictionary<Type, Action<object, int[], object>>();

        /// <summary>
        /// Builds a lambda function to read the elements of a generic array.
        /// </summary>
        public static Func<object, int[], object> GetArrayGetter(object array)
        {
            return GetArrayGetter(array, out _);
        }

        /// <summary>
        /// Builds a lambda function to read the elements of a generic array.
        /// </summary>
        public static Func<object, int[], object> GetArrayGetter(object array, out int[] dims)
        {
            var type = GetArrayDims(array, out dims);

            lock (arrayGetters)
            {
                if (!arrayGetters.TryGetValue(type, out var func))
                {
                    func = CreateArrayGetter(type);
                    arrayGetters.Add(type, func);
                }

                return func;
            }
        }

        /// <summary>
        /// Builds a lambda function to write the elements of a generic array.
        /// </summary>
        public static Action<object, int[], object> GetArraySetter(object array)
        {
            return GetArraySetter(array, out _);
        }

        /// <summary>
        /// Builds a lambda function to write the elements of a generic array.
        /// </summary>
        public static Action<object, int[], object> GetArraySetter(object array, out int[] dims)
        {
            var type = GetArrayDims(array, out dims);

            lock (arrayGetters)
            {
                if (!arraySetters.TryGetValue(type, out var lambda))
                {
                    lambda = CreateArraySetter(type);
                    arraySetters.Add(type, lambda);
                }

                return lambda;
            }
        }

        /// <summary>
        /// Get array dimensions.
        /// </summary>
        private static Type GetArrayDims(object array, out int[] dims)
        {
            var type = array.GetType();

            if (!type.IsArray)
            {
                throw new ArgumentException($"Object of type [{type.Name}] is not an array");
            }

            var rank = type.GetArrayRank();

            dims = new int[rank];
            for (var i = 0; i < rank; i++)
            {
                dims[i] = ((Array)array).GetLength(i);
            }

            return type;
        }

        /// <summary>
        /// Builds a lambda function to read the elements of a generic array.
        /// </summary>
        private static Func<object, int[], object> CreateArrayGetter(Type type)
        {
            var arrayParam = Expression.Parameter(typeof(object), "array");
            var indexesParam = Expression.Parameter(typeof(int[]), "indexes");

            var et = type.GetElementType();
            var rank = type.GetArrayRank();
            var castArray = Expression.TypeAs(arrayParam, et.MakeArrayType(rank));
            var variables = Enumerable.Range(0, rank).Select(i => Expression.ArrayAccess(indexesParam, Expression.Constant(i)));
            var arrayRead = Expression.TypeAs(Expression.ArrayAccess(castArray, variables), typeof(object));

            return Expression.Lambda<Func<object, int[], object>>(arrayRead, arrayParam, indexesParam).Compile();
        }

        /// <summary>
        /// Builds a lambda function to write the elements of a generic array.
        /// </summary>
        private static Action<object, int[], object> CreateArraySetter(Type type)
        {
            var arrayParam = Expression.Parameter(typeof(object), "array");
            var indexesParam = Expression.Parameter(typeof(int[]), "indexes");
            var valueParam = Expression.Parameter(typeof(object), "value");

            var et = type.GetElementType();
            var rank = type.GetArrayRank();
            var castArray = Expression.TypeAs(arrayParam, et.MakeArrayType(rank));
            var variables = Enumerable.Range(0, rank).Select(i => Expression.ArrayAccess(indexesParam, Expression.Constant(i)));
            var arrayWrite = Expression.Assign(Expression.ArrayAccess(castArray, variables), Expression.Convert(valueParam, et));

            return Expression.Lambda<Action<object, int[], object>>(arrayWrite, arrayParam, indexesParam, valueParam).Compile();
        }
    }
}
