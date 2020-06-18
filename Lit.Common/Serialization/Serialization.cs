using System;
using System.Text;
using Lit.DataType;

namespace Lit
{
    /// <summary>
    /// Values serialization.
    /// </summary>
    public static partial class Serialization
    {
        #region String constants

        private const char JsonStringQuotes = '"';

        private const string JsonNull = @"null";

        private const string JsonFalse = @"false";

        private const string JsonTrue = @"true";

        private const string JsonArrayStart = @"[";

        private const string JsonArrayEnd = @"]";

        private const string JsonObjectStart = @"{";

        private const string JsonObjectEnd = @"}";

        private const string JsonSeparator = ",";

        private const char CompactCharQuotes = '\'';

        private const char CompactStringQuotes = '"';

        private const string CompactNull = @"_";

        private const string CompactFalse = @"F";

        private const string CompactTrue = @"T";

        #endregion

        /// <summary>
        /// Encodes a generic.
        /// </summary>
        public static string Encode(object value)
        {
            return Encode(value, SerializationMode.Compact);
        }

        /// <summary>
        /// Encodes a generic.
        /// </summary>
        public static string Encode(object value, SerializationMode mode)
        {
            var sb = new StringBuilder();
            Encode(sb, value, mode);
            return sb.ToString();
        }

        /// <summary>
        /// Encodes a generic.
        /// </summary>
        public static void Encode(StringBuilder sb, object value, SerializationMode mode)
        {
            if (value == null)
            {
                EncodeNull(sb, mode);
                return;
            }

            var type = value.GetType();

            if (type == typeof(bool))
            {
                EncodeBoolean(sb, (bool)value, mode);
                return;
            }

            if (type == typeof(char))
            {
                EncodeChar(sb, (char)value, mode);
                return;
            }

            if (type == typeof(string))
            {
                EncodeString(sb, value as string, mode);
                return;
            }

            if (TypeHelper.IsInteger(type) || TypeHelper.IsFloatingPoint(type))
            {
                EncodeNumber(sb, value);
                return;
            }

            if (type.IsEnum)
            {
                EncodeEnum(sb, value);
                return;
            }

            if (type.IsArray)
            {
                EncodeArray(sb, value, mode);
                return;
            }

            throw new ArgumentException($"Serialization.Encode: unknown type{type.Name}");
        }
    }
}
