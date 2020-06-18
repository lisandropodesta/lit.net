using System.Text;

namespace Lit
{
    /// <summary>
    /// Numbers serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Encodes a number.
        /// </summary>
        public static void EncodeNumber(StringBuilder sb, object value)
        {
            sb.Append(EncodeNumber(value));
        }

        /// <summary>
        /// Encodes a number.
        /// </summary>
        public static string EncodeNumber(object value)
        {
            return value.ToString();
        }
    }
}
