using System;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    public abstract class DbTranslation : IDbTranslation
    {
        /// <summary>
        /// Encodes a C# value into a DB value.
        /// </summary>
        public abstract object ToDb(DbDataType dataType, Type type, object value);

        /// <summary>
        /// Decodes a DB value into a C# value.
        /// </summary>
        public abstract object FromDb(DbDataType dataType, Type type, object value);

        /// <summary>
        /// Translates a common scalar to DB.
        /// </summary>
        public static object ScalarToDb(Type type, object value)
        {
            return TypeHelper.GetEnumAttribute<IDbCode>(type, value, out var attr) ? attr.Code : value;
        }

        /// <summary>
        /// Translates a common scalar from DB.
        /// </summary>
        public static object ScalarFromDb(Type type, object value)
        {
            if (type == value.GetType())
            {
                return value;
            }

            if (value is string)
            {
                if (type.IsEnum && FindDbEnumCode(type, value as string, out var enumValue))
                {
                    return enumValue;
                }

                if (type == typeof(bool))
                {
                    return bool.Parse((string)value);
                }

                if (TypeHelper.IsInteger(type))
                {
                    return long.Parse((string)value);
                }

                if (TypeHelper.IsFloatingPoint(type))
                {
                    return double.Parse((string)value);
                }

                return value;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }

            return Convert.ChangeType(value, type);
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
                    if (TypeHelper.GetAttribute<IDbCode>(fieldInfo, out var dbCodeAttr))
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
