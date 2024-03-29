﻿using System;
using System.Collections.Generic;

namespace Lit.DataType
{
    public static partial class TypeHelper
    {
        /// <summary>
        /// Adds a binding.
        /// </summary>
        public static TI AddBinding<TI, TA>(ref TypeBinding<TI, TA> bindings, Type genericType, Type[] typeArguments, params object[] instanceArguments)
            where TI : class
            where TA : class
        {
            if (bindings == null)
            {
                bindings = new TypeBinding<TI, TA>();
            }

            var binding = CreateInstance(genericType, typeArguments, instanceArguments) as TI;
            bindings.AddBinding(binding);
            return binding;
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        public static TI AddBinding<TI>(List<TI> bindings, Type genericType, Type[] typeArguments, params object[] instanceArguments)
            where TI : class
        {
            var binding = CreateInstance(genericType, typeArguments, instanceArguments) as TI;
            bindings.Add(binding);
            return binding;
        }

        /// <summary>
        /// Gets the binding kind of a property type.
        /// </summary>
        public static BindingMode GetBindingMode(ref Type type, out bool isNullable)
        {
            if (TypeHelper.IsScalarType(type))
            {
                isNullable = type == typeof(string);
                return BindingMode.Scalar;
            }

            isNullable = true;

            if (type.IsGenericType && type.GetGenericArguments().Length == 1)
            {
                var gdef = type.GetGenericTypeDefinition();
                var gtype = type.GetGenericArguments()[0];

                if (gdef == typeof(Nullable<>))
                {
                    if (TypeHelper.IsScalarType(gtype))
                    {
                        type = gtype;
                        return BindingMode.Scalar;
                    }
                }
                else if (TypeHelper.IsGenericList(gdef))
                {
                    type = gtype;
                    if (GetBindingMode(ref gtype, out _) == BindingMode.Class)
                    {
                        return BindingMode.ClassList;
                    }

                    return BindingMode.List;
                }
            }

            if (TypeHelper.IsDictionary(type))
            {
                return BindingMode.Dictionary;
            }

            if (type.IsClass && !type.IsInterface)
            {
                return BindingMode.Class;
            }

            return BindingMode.None;
        }

        /// <summary>
        /// Check if the type is a supported scalar type.
        /// </summary>
        public static bool IsScalarType(Type type)
        {
            return type.IsEnum || ScalarTypes.Contains(type);
        }

        private static readonly List<Type> ScalarTypes = new List<Type>
        {
            typeof(bool),
            typeof(char),

            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),

            typeof(float),
            typeof(double),
            typeof(decimal),

            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(string),
            typeof(byte[])
        };

        /// <summary>
        /// Check if the type is an integer type.
        /// </summary>
        public static bool IsInteger(Type type)
        {
            return IntegerTypes.Contains(type);
        }

        private static readonly List<Type> IntegerTypes = new List<Type>
        {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong)
        };

        /// <summary>
        /// Check if the type is a floating point type.
        /// </summary>
        public static bool IsFloatingPoint(Type type)
        {
            return FloatingPointTypes.Contains(type);
        }

        private static readonly List<Type> FloatingPointTypes = new List<Type>
        {
            typeof(float),
            typeof(double)
        };

        /// <summary>
        /// Check if the type is a generic list.
        /// </summary>
        public static bool IsGenericList(Type type)
        {
            return GenericListTypes.Contains(type);
        }

        private static readonly List<Type> GenericListTypes = new List<Type>
        {
            typeof(List<>),
            typeof(IList<>),
            typeof(IReadOnlyList<>),
            typeof(ICollection<>),
            typeof(IReadOnlyCollection<>),
            typeof(IEnumerable<>)
        };

        /// <summary>
        /// Check if the type is a dictionary.
        /// </summary>
        public static bool IsDictionary(Type type)
        {
            return DictionaryTypes.Contains(type);
        }

        private static readonly List<Type> DictionaryTypes = new List<Type>
        {
            typeof(Dictionary<string,object>),
            typeof(IDictionary<string,object>)
        };
    }
}
