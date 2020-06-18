using System;
using System.Text;

namespace Lit
{
    /// <summary>
    /// Strings serialization.
    /// </summary>
    public static partial class Serialization
    {
        /// <summary>
        /// Encodes a char.
        /// </summary>
        public static void EncodeChar(StringBuilder sb, char value, SerializationMode mode)
        {
            sb.Append(EncodeChar(value, mode));
        }

        /// <summary>
        /// Encodes a char.
        /// </summary>
        public static string EncodeChar(char value, SerializationMode mode)
        {
            switch (mode)
            {
                case SerializationMode.Compact:
                default:
                    return CompactCharQuotes.ToString() + value + CompactCharQuotes;

                case SerializationMode.JsonCompact:
                case SerializationMode.JsonPrintable:
                    return EncodeString(value.ToString(), mode);
            }
        }

        /// <summary>
        /// Encodes a string.
        /// </summary>
        public static string EncodeString(string text, SerializationMode mode)
        {
            var sb = new StringBuilder((text?.Length ?? 0) + 2);
            EncodeString(sb, text, mode);
            return sb?.ToString();
        }

        /// <summary>
        /// Encodes a string.
        /// </summary>
        public static void EncodeString(StringBuilder sb, string text, SerializationMode mode)
        {
            char quotesChar;
            switch (mode)
            {
                case SerializationMode.Compact:
                default:
                    quotesChar = CompactStringQuotes;
                    break;

                case SerializationMode.JsonCompact:
                case SerializationMode.JsonPrintable:
                    quotesChar = JsonStringQuotes;
                    break;
            }

            EncodeString(sb, text, quotesChar);
        }

        /// <summary>
        /// Encodes a string.
        /// </summary>
        public static void EncodeString(StringBuilder sb, string text, char quotesChar)
        {
            var hasQuotesChar = quotesChar != '\0';
            var len = text?.Length ?? 0;

            if (hasQuotesChar)
            {
                sb.Append(quotesChar);
            }

            for (var i = 0; i < len; i++)
            {
                var c = text[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;

                    case '\\':
                        sb.Append(@"\\");
                        break;

                    case '\0':
                        sb.Append(@"\0");
                        break;

                    case '\a':
                        sb.Append(@"\a");
                        break;

                    case '\b':
                        sb.Append(@"\b");
                        break;

                    case '\f':
                        sb.Append(@"\f");
                        break;

                    case '\n':
                        sb.Append(@"\n");
                        break;

                    case '\r':
                        sb.Append(@"\r");
                        break;

                    case '\t':
                        sb.Append(@"\t");
                        break;

                    case '\v':
                        sb.Append(@"\v");
                        break;

                    default:
                        if (c >= 0x20 && c <= 0x7e)
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            sb.Append(@"\u");
                            sb.Append(((ushort)c).ToString("x4"));
                        }
                        break;
                }
            }

            if (hasQuotesChar)
            {
                sb.Append(quotesChar);
            }
        }

        /// <summary>
        /// Decodes a string.
        /// </summary>
        public static string DecodeString(string text, SerializationMode mode)
        {
            return DecodeString(text, '"', mode);
        }

        /// <summary>
        /// Decodes a string.
        /// </summary>
        public static string DecodeString(string text, char quotesChar, SerializationMode mode)
        {
            StringBuilder sb = null;

            var hasQuotesChar = quotesChar != '\0';
            var len = text?.Length ?? 0;

            if (len > 0)
            {
                sb = new StringBuilder(len);

                var f = 0;
                var t = len - 1;
                if (hasQuotesChar)
                {
                    if (len < 2 || text[0] != quotesChar || text[len - 1] != quotesChar)
                    {
                        throw new ArgumentException($"DecodeString: invalid quotes on string [{text}].");
                    }

                    f = 1;
                    t = len - 2;
                }

                var error = false;
                for (var i = f; i <= t; i++)
                {
                    var c = text[i];
                    if (c != '\\')
                    {
                        sb.Append(c);
                    }
                    else if (i < t)
                    {
                        c = text[++i];
                        switch (c)
                        {
                            case '"':
                            case '\\':
                                sb.Append(c);
                                break;

                            case '0':
                                sb.Append('\0');
                                break;

                            case 'a':
                                sb.Append('\a');
                                break;

                            case 'b':
                                sb.Append('\b');
                                break;

                            case 'f':
                                sb.Append('\f');
                                break;

                            case 'n':
                                sb.Append('\n');
                                break;

                            case 'r':
                                sb.Append('\r');
                                break;

                            case 't':
                                sb.Append('\t');
                                break;

                            case 'v':
                                sb.Append('\v');
                                break;

                            case 'u':
                                if (i + 4 <= t)
                                {
                                    sb.Append(char.ConvertFromUtf32(Convert.ToInt32(text.Substring(i + 1, 4), 16)));
                                    i += 4;
                                }
                                else
                                {
                                    error = true;
                                }
                                break;

                            default:
                                error = true;
                                break;
                        }
                    }
                    else
                    {
                        error = true;
                    }

                    if (error)
                    {
                        throw new ArgumentException($"DecodeString: invalid format on string [{text}].");
                    }
                }
            }

            return sb?.ToString() ?? string.Empty;
        }
    }
}
