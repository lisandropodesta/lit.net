using System;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    public class DbTranslation : IDbTranslation
    {
        /// <summary>
        /// Encodes a C# value into a DB value.
        /// </summary>
        public virtual object ToDb<T>(BindingMode Mode, Type dstType, T value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            switch (Mode)
            {
                case BindingMode.Scalar:
                    if (TypeHelper.GetEnumAttribute<DbEnumCodeAttribute>(dstType, value, out var attr))
                    {
                        return attr.Code;
                    }
                    break;

                case BindingMode.Class:
                case BindingMode.List:
                case BindingMode.Dictionary:
                default:
                    throw new NotImplementedException();
            }

            return value;
        }

        /// <summary>
        /// Decodes a DB value into a C# value.
        /// </summary>
        public virtual T FromDb<T>(BindingMode Mode, Type srcType, object value)
        {
            if (value == null || value is DBNull)
            {
                return default;
            }

            var type = srcType;

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
                            else if (TypeHelper.IsInteger(type))
                            {
                                value = long.Parse((string)value);
                            }
                            else if (TypeHelper.IsFloatingPoint(type))
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
                    throw new NotImplementedException();
            }

            return (T)value;
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
