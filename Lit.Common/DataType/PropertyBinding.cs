using System;
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

        public PropertyBinding(PropertyInfo propInfo, bool getterRequired = false, bool setterRequired = false, bool? isNullableForced = null)
            : base(propInfo)
        {
            bindingType = propInfo.PropertyType;
            mode = TypeHelper.GetBindingMode(ref bindingType, out isNullable);

            if (isNullableForced.HasValue)
            {
                isNullable = isNullableForced.Value;
            }

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
        /// Sets the binding value to an instance.
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
    }
}
