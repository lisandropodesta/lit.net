using System;

namespace Lit.DataType
{
    /// <summary>
    /// Binding to a class's property.
    /// </summary>
    public interface IPropertyBinding
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Binding mode.
        /// </summary>
        BindingMode Mode { get; }

        /// <summary>
        /// Binding type.
        /// </summary>
        Type BindingType { get; }

        /// <summary>
        /// Is nullable flag.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        /// Gets the binding value from an instance.
        /// </summary>
        object GetRawValue(object instance);

        /// <summary>
        /// Sets the binding value to an instance.
        /// </summary>
        void SetRawValue(object instance, object value);

        /// <summary>
        /// Gets the binding value from an instance.
        /// </summary>
        object GetValue(object instance);

        /// <summary>
        /// Sets the binding value to an instance.
        /// </summary>
        void SetValue(object instance, object value);

        /// <summary>
        /// Calculates binding mode.
        /// </summary>
        void CalcBindingMode();
    }
}
