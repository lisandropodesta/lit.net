using System;
using Lit.DataType;

namespace Lit.Db
{
    public abstract class DbTranslation : IDbTranslation
    {
        /// <summary>
        /// Encodes a C# value into a DB value.
        /// </summary>
        public abstract object ToDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value);

        /// <summary>
        /// Decodes a DB value into a C# value.
        /// </summary>
        public abstract object FromDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value);

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

            if (value is string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                if (type.IsEnum)
                {
                    return GetDbEnumCode(type, text);
                }

                if (type == typeof(bool))
                {
                    return bool.Parse(text);
                }

                if (TypeHelper.IsInteger(type))
                {
                    return long.Parse(text);
                }

                if (TypeHelper.IsFloatingPoint(type))
                {
                    return double.Parse(text);
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
        public static object GetDbEnumCode(Type enumType, string targetValue)
        {
            return Serialization.DecodeEnum<IDbCode>(targetValue, enumType, (t, a) => targetValue == a?.Code);
        }
    }
}
