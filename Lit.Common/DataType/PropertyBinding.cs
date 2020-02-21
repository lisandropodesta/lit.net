using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// Binding to a specific property of a specific class.
    /// </summary>
    public class PropertyBinding<TC, TP> : ReflectionProperty, IPropertyBinding where TC : class
    {
        /// <summary>
        /// Binding mode.
        /// </summary>
        public BindingMode Mode => mode;

        private readonly BindingMode mode;

        /// <summary>
        /// Binding type.
        /// </summary>
        public Type BindingType => bindingType;

        private readonly Type bindingType;

        /// <summary>
        /// Is nullable flag.
        /// </summary>
        public bool IsNullable => isNullable;

        private readonly bool isNullable;

        private readonly Action<TC, TP> setter;

        private readonly Func<TC, TP> getter;

        #region Constructor

        public PropertyBinding(PropertyInfo propInfo, bool getterRequired = false, bool setterRequired = false)
            : base(propInfo)
        {
            bindingType = propInfo.PropertyType;
            mode = GetBindingMode(ref bindingType, out isNullable);

            var gm = propInfo.GetGetMethod(true);
            if (gm != null)
            {
                getter = (Func<TC, TP>)Delegate.CreateDelegate(typeof(Func<TC, TP>), null, gm);
            }
            else if (getterRequired)
            {
                throw new ArgumentException($"Property{this} has no getter method.");
            }

            var sm = propInfo.GetSetMethod(true);
            if (sm != null)
            {
                setter = (Action<TC, TP>)Delegate.CreateDelegate(typeof(Action<TC, TP>), null, sm);
            }
            else if (setterRequired)
            {
                throw new ArgumentException($"Property{this} has no setter method.");
            }
        }

        #endregion

        /// <summary>
        /// Gets the binding value from an instance.
        /// </summary>
        public object GetValue(object instance)
        {
            if (getter == null)
            {
                throw new ArgumentException($"Property{this} has no getter method.");
            }

            var value = getter(instance as TC);

            return DecodePropertyValue(value);
        }

        /// <summary>
        /// Sets the binding value to an instances.
        /// </summary>
        public void SetValue(object instance, object value)
        {
            if (setter == null)
            {
                throw new ArgumentException($"Property{this} has no setter method.");
            }

            var propValue = EncodePropertyValue(value);

            setter(instance as TC, propValue);
        }

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected virtual object DecodePropertyValue(TP value)
        {
            return value;
        }

        /// <summary>
        /// Value encoding to property type.
        /// </summary>
        protected virtual TP EncodePropertyValue(object value)
        {
            return value != null ? (TP)value : default;
        }

        #region Public static methods

        /// <summary>
        /// Gets the binding kind of a property type.
        /// </summary>
        public static BindingMode GetBindingMode(ref Type type, out bool isNullable)
        {
            isNullable = false;

            if (IsScalarType(type))
            {
                return BindingMode.Scalar;
            }

            if (type.IsGenericType && type.GetGenericArguments().Length == 1)
            {
                var gdef = type.GetGenericTypeDefinition();
                var gtype = type.GetGenericArguments()[0];

                if (gdef == typeof(Nullable<>))
                {
                    isNullable = true;

                    if (IsScalarType(gtype))
                    {
                        type = gtype;
                        return BindingMode.Scalar;
                    }
                }
                else if (IsGenericList(gdef))
                {
                    type = gtype;
                    return BindingMode.List;
                }
            }

            if (IsDictionary(type))
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

        #endregion
    }
}
