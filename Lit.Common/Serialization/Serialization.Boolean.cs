using System.Text;

namespace Lit
{
    /// <summary>
    /// Boolean serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Encodes a boolean.
        /// </summary>
        public static void EncodeBoolean(StringBuilder sb, bool value, SerializationMode mode)
        {
            sb.Append(EncodeBoolean(value, mode));
        }

        /// <summary>
        /// Encodes a boolean.
        /// </summary>
        public static string EncodeBoolean(bool value, SerializationMode mode)
        {
            switch (mode)
            {
                case SerializationMode.Compact:
                default:
                    return value ? CompactTrue : CompactFalse;

                case SerializationMode.JsonCompact:
                case SerializationMode.JsonPrintable:
                    return value ? JsonTrue : JsonFalse;
            }
        }
    }
}
