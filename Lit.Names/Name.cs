using System;
using System.Text;

namespace Lit.Names
{
    /// <summary>
    /// Name handling utility.
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Translates a name.
        /// </summary>
        public static string Translate(string name, Case namingCase, AffixPlacing idPlacing, string idText)
        {
            var words = SplitInWords(name, namingCase);
            var count = words.Length;

            var startIndex = 0;
            if (count > 1 && !string.IsNullOrEmpty(idText))
            {
                switch (idPlacing)
                {
                    case AffixPlacing.DoNotChange:
                    default:
                        break;

                    case AffixPlacing.DoNotPlace:
                        if (string.Compare(words[0], idText, true) != 0)
                        {
                            words[0] = string.Empty;
                        }
                        if (string.Compare(words[count - 1], idText, true) == 0)
                        {
                            words[count - 1] = string.Empty;
                        }
                        break;

                    case AffixPlacing.Prefix:
                        if (string.Compare(words[0], idText, true) != 0 && string.Compare(words[count - 1], idText, true) == 0)
                        {
                            startIndex = count - 1;
                            words[startIndex] = idText;
                        }
                        break;

                    case AffixPlacing.Sufix:
                        if (string.Compare(words[0], idText, true) == 0 && string.Compare(words[count - 1], idText, true) != 0)
                        {
                            startIndex = 1;
                            words[0] = idText;
                        }
                        break;

                    case AffixPlacing.Whole:
                        if (string.Compare(words[0], idText, true) != 0 || string.Compare(words[count - 1], idText, true) == 0)
                        {
                            return idText;
                        };
                        break;
                }
            }

            var separator = GetSeparator(namingCase);

            var result = new StringBuilder();

            var first = true;
            for (var index = 0; index < count; index++)
            {
                var word = words[(index + startIndex) % count];

                if (!string.IsNullOrEmpty(word))
                {
                    if (!first)
                    {
                        result.Append(separator);
                    }

                    result.Append(word);
                    first = false;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Split in words, assumes a valid input case.
        /// </summary>
        public static string[] SplitInWords(string name, Case namingCase)
        {
            var length = name.Length;
            var wordCount = 0;
            var strings = new string[0];
            var word = string.Empty;
            var tail = string.Empty;

            bool addChar;
            bool delayChar;
            bool flushWord;
            bool flushAll;
            var isDigits = false;
            var allCaps = false;

            for (var i = 0; i <= length; i++)
            {
                var c = i < length ? name[i] : ' ';

                addChar = false;
                delayChar = false;
                flushWord = false;
                flushAll = false;

                if (!Char.IsLetterOrDigit(c))
                {
                    flushAll = true;
                }
                else if (word == string.Empty)
                {
                    if (c != ' ')
                    {
                        addChar = true;
                        allCaps = Char.IsUpper(c);
                        isDigits = Char.IsDigit(c);
                    }
                }
                else if (Char.IsUpper(c))
                {
                    if (allCaps)
                    {
                        delayChar = true;
                    }
                    else
                    {
                        flushAll = true;
                        delayChar = true;
                    }
                }
                else
                {
                    if (Char.IsDigit(c))
                    {
                        addChar = true;
                        isDigits = true;
                    }
                    else if (isDigits)
                    {
                        flushAll = true;
                        delayChar = true;
                    }
                    else if (allCaps && word.Length + tail.Length > 1)
                    {
                        flushWord = true;
                        delayChar = true;
                    }
                    else
                    {
                        addChar = true;
                    }

                    allCaps = false;
                }

                if (addChar)
                {
                    tail += c;
                }

                if (flushAll || !flushWord)
                {
                    if (tail.Length > 0)
                    {
                        AddText(ref word, tail, namingCase, wordCount == 0);
                        tail = string.Empty;
                    }
                }

                if (delayChar)
                {
                    tail += c;
                }

                if (flushAll || flushWord)
                {
                    if (word.Length > 0)
                    {
                        Array.Resize(ref strings, wordCount + 1);
                        strings[wordCount++] = word;
                        word = string.Empty;
                    }

                    i -= tail.Length;
                    tail = string.Empty;
                }
            }

            return strings;
        }

        /// <summary>
        /// Adds text to a word formatting according a naming case.
        /// </summary>
        public static void AddText(ref string word, string text, Case namingCase, bool firstWord)
        {
            for (var j = 0; j < text.Length; j++)
            {
                var c = text[j];

                switch (namingCase)
                {
                    case Case.Camel:
                        c = word == string.Empty && !firstWord ? Char.ToUpper(c) : Char.ToLower(c);
                        break;

                    case Case.Pascal:
                        c = word == string.Empty ? Char.ToUpper(c) : Char.ToLower(c);
                        break;

                    case Case.UpperSnake:
                    case Case.UpperKebab:
                        c = Char.ToUpper(c);
                        break;

                    case Case.Snake:
                    case Case.Kebab:
                        c = Char.ToLower(c);
                        break;

                    case Case.Unspecified:
                    default:
                        break;
                }

                word += c;
            }
        }

        /// <summary>
        /// Get the words separator of the naming case.
        /// </summary>
        public static string GetSeparator(Case namingCase)
        {
            switch (namingCase)
            {
                case Case.Snake:
                case Case.UpperSnake:
                    return "_";

                case Case.Kebab:
                case Case.UpperKebab:
                    return "-";

                case Case.Unspecified:
                case Case.Camel:
                case Case.Pascal:
                default:
                    return string.Empty;
            }
        }
    }
}
