using System;
using System.Text;
using Lit.DataType;

namespace Lit
{
    /// <summary>
    /// Array serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Encodes an array.
        /// </summary>
        public static string EncodeArray(object value, SerializationMode mode)
        {
            var sb = new StringBuilder();
            EncodeArray(sb, value, mode);
            return sb?.ToString();
        }

        /// <summary>
        /// Encodes an array.
        /// </summary>
        public static void EncodeArray(StringBuilder sb, object value, SerializationMode mode)
        {
            var getter = TypeHelper.GetArrayGetter(value, out int[] dims);
            var indexes = new int[dims.Length];
            EncodeArray(sb, value, mode, 0, indexes, dims, getter);
        }

        /// <summary>
        /// Recursive encoding of an array.
        /// </summary>
        private static void EncodeArray(StringBuilder sb, object value, SerializationMode mode,
            int di, int[] indexes, int[] dims, Func<object, int[], object> getter)
        {
            var separator = GetArraySeparator(sb, mode);

            sb.Append(JsonArrayStart);

            for (indexes[di] = 0; indexes[di] < dims[di]; indexes[di]++)
            {
                if (indexes[di] > 0)
                {
                    sb.Append(separator);
                }

                if (di < dims.Length - 1)
                {
                    EncodeArray(sb, value, mode, di + 1, indexes, dims, getter);
                }
                else
                {
                    Encode(sb, getter(value, indexes), mode);
                }
            }

            sb.Append(JsonArrayEnd);
        }

        /// <summary>
        /// Get array separator.
        /// </summary>
        private static string GetArraySeparator(StringBuilder sb, SerializationMode mode)
        {
            switch (mode)
            {
                case SerializationMode.Compact:
                case SerializationMode.JsonCompact:
                default:
                    return JsonSeparator;

                case SerializationMode.JsonPrintable:
                    // TODO: add indentation
                    return JsonSeparator + " ";
            }
        }
    }
}
