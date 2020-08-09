using System;

namespace Lit.DataType
{
    public static partial class TypeHelper
    {
        /// <summary>
        /// Action types.
        /// </summary>
        public static readonly Type[] ActionTypes = new Type[]
        {
            typeof(Action),
            typeof(Action<>),
            typeof(Action<,>),
            typeof(Action<,,>),
            typeof(Action<,,,>),
            typeof(Action<,,,,>),
            typeof(Action<,,,,,>),
            typeof(Action<,,,,,,>),
            typeof(Action<,,,,,,,>),
            typeof(Action<,,,,,,,,>),
            typeof(Action<,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,,,>)
        };

        /// <summary>
        /// Func types.
        /// </summary>
        public static readonly Type[] FuncTypes = new Type[]
        {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,,>)
        };

        /// <summary>
        /// Makes a delegate type based on argument types.
        /// The last argmentType is return value type.
        /// </summary>
        public static Type MakeDelegateType(Type[] argumentTypes)
        {
            var genericType = GetGenericDelegateType(argumentTypes);

            var typesArray = argumentTypes;
            if (argumentTypes[argumentTypes.Length - 1] == null)
            {
                typesArray = new Type[argumentTypes.Length - 1];
                Array.Copy(argumentTypes, typesArray, argumentTypes.Length - 1);
            }

            return genericType.MakeGenericType(typesArray);
        }

        /// <summary>
        /// Gets a generic delegate type based on argument types.
        /// The last argmentType is return value type.
        /// </summary>
        public static Type GetGenericDelegateType(Type[] argumentTypes)
        {
            var paramsCount = argumentTypes.Length - 1;
            if (argumentTypes[paramsCount] == null)
            {
                if (paramsCount < ActionTypes.Length)
                {
                    return ActionTypes[paramsCount];
                }
            }
            else
            {
                if (paramsCount < FuncTypes.Length)
                {
                    return FuncTypes[paramsCount];
                }
            }

            throw new ArgumentException("Invalid delegate types.");
        }

        /// <summary>
        /// Builds a generic type a creates an instace.
        /// </summary>
        public static object CreateInstance(Type genericType, Type[] typeArguments, params object[] instanceArguments)
        {
            var type = genericType.MakeGenericType(typeArguments);
            return Activator.CreateInstance(type, instanceArguments);
        }
    }
}
