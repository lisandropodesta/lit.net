using System;
using System.Text;
using Lit.DataType;

namespace Lit
{
    /// <summary>
    /// Enums serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Encodes an enum value.
        /// </summary>
        public static void EncodeEnum(StringBuilder sb, object value)
        {
            sb.Append(EncodeEnum(value));
        }

        /// <summary>
        /// Encodes an enum value.
        /// </summary>
        public static string EncodeEnum(object value)
        {
            return EncodeEnum<SerializationAttribute>(value, (o, a) => a?.EncodedText ?? Enum.GetName(value.GetType(), value));
        }

        /// <summary>
        /// Encodes an enum value.
        /// </summary>
        public static void EncodeEnum<TA>(StringBuilder sb, object value, Func<object, TA, string> attrEncoder) where TA : class
        {
            sb.Append(EncodeEnum(value, attrEncoder));
        }

        /// <summary>
        /// Encodes an enum value.
        /// </summary>
        public static string EncodeEnum<TA>(object value, Func<object, TA, string> attrEncoder) where TA : class
        {
            if (value == null)
            {
                return string.Empty;
            }

            var attr = TypeHelper.TryGetEnumAttribute<TA>(value?.GetType(), value);
            return attrEncoder(value, attr);
        }

        /// <summary>
        /// Encodes an enum value.
        /// </summary>
        public static object DecodeEnum(string text, Type enumType)
        {
            return DecodeEnum<SerializationAttribute>(text, enumType, (s, a) => a?.Match(text) ?? false);
        }

        /// <summary>
        /// Decodes an enum value.
        /// </summary>
        public static object DecodeEnum<TA>(string text, Type enumType, Func<string, TA, bool> attrDecoder) where TA : class
        {
            if (!string.IsNullOrEmpty(text) && Enum.IsDefined(enumType, text))
            {
                return Enum.Parse(enumType, text);
            }

            if (attrDecoder != null)
            {
                foreach (var fieldInfo in enumType.GetFields())
                {
                    if (fieldInfo.IsLiteral && TypeHelper.GetAttribute<TA>(fieldInfo, out var attr))
                    {
                        if (attrDecoder(text, attr))
                        {
                            return fieldInfo.GetValue(null);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            throw new ArgumentException($"DecodeEnum: invalid enum value [{text}] for type [{enumType.Name}].");
        }
    }
}
