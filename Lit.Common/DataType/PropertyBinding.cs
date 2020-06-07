using System;
using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// Binding to a class's property.
    /// </summary>
    public class PropertyBinding<TC, TP> : ReflectionProperty, IPropertyBinding where TC : class
    {
        /// <summary>
        /// Shortcut for property name.
        /// </summary>
        public string PropertyName => PropertyInfo.Name;

        /// <summary>
        /// Binding mode.
        /// </summary>
        public BindingMode Mode { get; private set; }

        /// <summary>
        /// Binding type.
        /// </summary>
        public Type BindingType { get; private set; }

        /// <summary>
        /// Is nullable flag.
        /// </summary>
        public bool IsNullable { get; private set; }

        private readonly Action<TC, TP> setter;

        private readonly Func<TC, TP> getter;

        #region Constructor

        public PropertyBinding(PropertyInfo propInfo, bool getterRequired = false, bool setterRequired = false)
            : base(propInfo)
        {
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
        public object GetRawValue(object instance)
        {
            if (getter == null)
            {
                throw new ArgumentException($"Property{this} has no getter method.");
            }

            return getter(instance as TC);
        }

        /// <summary>
        /// Sets the binding value to an instance.
        /// </summary>
        public void SetRawValue(object instance, object value)
        {
            if (setter == null)
            {
                throw new ArgumentException($"Property{this} has no setter method.");
            }

            var propValue = value != null ? (TP)value : default;
            setter(instance as TC, propValue);
        }

        /// <summary>
        /// Gets the binding value from an instance.
        /// </summary>
        public virtual object GetValue(object instance)
        {
            return DecodePropertyValue((TP)GetRawValue(instance));
        }

        /// <summary>
        /// Sets the binding value to an instance.
        /// </summary>
        public virtual void SetValue(object instance, object value)
        {
            value = EncodePropertyValue(value);
            SetRawValue(instance, value);
        }

        /// <summary>
        /// Calculates binding mode.
        /// </summary>
        public void CalcBindingMode()
        {
            var bindingType = PropertyInfo.PropertyType;
            Mode = GetBindingMode(ref bindingType, out bool isNullable);
            BindingType = bindingType;
            IsNullable = isNullable;
        }

        /// <summary>
        /// Get the binding mode.
        /// </summary>
        protected virtual BindingMode GetBindingMode(ref Type bindingType, out bool isNullable)
        {
            return TypeHelper.GetBindingMode(ref bindingType, out isNullable);
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
