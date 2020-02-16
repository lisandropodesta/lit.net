using System;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db property binding.
    /// </summary>
    internal interface IDbPropertyBinding<TA>
    {
        TA Attributes { get; }
    }

    /// <summary>
    /// Binding to a database property (parameter/field).
    /// </summary>
    internal abstract class DbPropertyBinding<TC, TP, TA> : PropertyBinding<TC, TP>, IDbPropertyBinding<TA> where TC : class where TA : Attribute
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        public TA Attributes => attr;

        private readonly TA attr;

        #region Constructor

        protected DbPropertyBinding(PropertyInfo propInfo, TA attr)
            : base(propInfo, true, true)
        {
            this.attr = attr;

            if (Mode == BindingMode.None)
            {
                throw new ArgumentException($"Property {this} of type [{propInfo.PropertyType.Name}] has an unknown binding mode");
            }
        }

        #endregion

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected override object DecodePropertyValue(TP value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            var type = BindingType;

            switch (Mode)
            {
                case BindingMode.Scalar:
                    if (TypeHelper.GetEnumAttribute<DbEnumCodeAttribute>(type, value, out var attr))
                    {
                        return attr.Code;
                    }
                    break;

                case BindingMode.Class:
                case BindingMode.List:
                case BindingMode.Dictionary:
                default:
                    throw new ArgumentException($"Property {this} of type [{PropertyInfo.PropertyType.Name}] has a unsupported binding {BindingType}.");
            }

            return value;
        }

        /// <summary>
        /// Value encoding to property type.
        /// </summary>
        protected override TP EncodePropertyValue(object value)
        {
            if (value == null || value is DBNull)
            {
                return default;
            }

            var type = BindingType;

            switch (Mode)
            {
                case BindingMode.Scalar:
                    if (type != value.GetType())
                    {
                        if (value is string)
                        {
                            if (type.IsEnum && FindDbEnumCode(type, value as string, out var enumValue))
                            {
                                value = enumValue;
                            }
                            else if (type == typeof(bool))
                            {
                                value = bool.Parse((string)value);
                            } 
                            else if (IsInteger(type))
                            {
                                value = long.Parse((string)value);
                            }
                            else if (IsFloatingPoint(type))
                            {
                                value = double.Parse((string)value);
                            }
                        }
                        else if (type.IsEnum)
                        {
                            value = Enum.Parse(type, value.ToString());
                        }
                        else
                        {
                            value = Convert.ChangeType(value, type);
                        }
                    }
                    break;

                case BindingMode.Class:
                case BindingMode.List:
                case BindingMode.Dictionary:
                    break;

                default:
                    throw new ArgumentException($"Property {this} of type [{PropertyInfo.PropertyType.Name}] has a unsupported binding {BindingType}.");
            }

            return (TP)value;
        }

        /// <summary>
        /// Finds a db code in an enum type.
        /// </summary>
        public static bool FindDbEnumCode(Type enumType, string targetValue, out object enumValue)
        {
            if (enumType.IsEnum && targetValue != null)
            {
                if (Enum.IsDefined(enumType, targetValue))
                {
                    enumValue = Enum.Parse(enumType, targetValue);
                    return true;
                }

                foreach (var fieldInfo in enumType.GetFields())
                {
                    if (TypeHelper.GetAttribute<DbEnumCodeAttribute>(fieldInfo, out var dbCodeAttr))
                    {
                        if (targetValue == dbCodeAttr.Code)
                        {
                            enumValue = fieldInfo.GetValue(null);
                            return true;
                        }
                    }
                }
            }

            enumValue = null;
            return false;
        }
    }
}
