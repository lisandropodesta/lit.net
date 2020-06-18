using System.Text;

namespace Lit
{
    /// <summary>
    /// Null serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Null encode.
        /// </summary>
        public static void EncodeNull(StringBuilder sb, SerializationMode mode)
        {
            sb.Append(EncodeNull(mode));
        }

        /// <summary>
        /// Null encode.
        /// </summary>
        public static string EncodeNull(SerializationMode mode)
        {
            switch (mode)
            {
                case SerializationMode.Compact:
                default:
                    return CompactNull;

                case SerializationMode.JsonCompact:
                case SerializationMode.JsonPrintable:
                    return JsonNull;
            }
        }
    }
}
