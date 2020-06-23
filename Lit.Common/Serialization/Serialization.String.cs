using System.Text;
using Newtonsoft.Json;

namespace Lit
{
    /// <summary>
    /// Strings serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Setup.
        /// </summary>
        public static void Setup()
        {
        }

        /// <summary>
        /// Encodes a char.
        /// </summary>
        public static void EncodeChar(StringBuilder sb, char value)
        {
            EncodeString(sb, value.ToString());
        }

        /// <summary>
        /// Encodes a string.
        /// </summary>
        public static void EncodeString(StringBuilder sb, string text)
        {
            sb.Append(JsonConvert.SerializeObject(text));
        }

        /// <summary>
        /// Decodes a string.
        /// </summary>
        public static string DecodeString(string text)
        {
            return JsonConvert.DeserializeObject<string>(text);
        }
    }
}
