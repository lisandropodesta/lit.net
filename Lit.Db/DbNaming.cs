using System;
using System.Reflection;
using System.Text;

namespace Lit.Db
{
    /// <summary>
    /// Common naming conventions.
    /// </summary>
    public class DbNaming : IDbNaming
    {
        #region Helper enums

        /// <summary>
        /// Base name source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// Configuration is preferred but in case it is missing then usess relection (this is the default).
            /// </summary>
            Default,

            /// <summary>
            /// Only uses configuration.
            /// </summary>
            Configuration,

            /// <summary>
            /// Only uses reflection
            /// </summary>
            Reflection
        }

        /// <summary>
        /// Translation scope.
        /// </summary>
        public enum Translation
        {
            /// <summary>
            /// None source is translated.
            /// </summary>
            None,

            /// <summary>
            /// Only reflection is translated.
            /// </summary>
            Reflection,

            /// <summary>
            /// Only configuration is translated.
            /// </summary>
            Configuration,

            /// <summary>
            /// Allo sources are translated.
            /// </summary>
            All
        }

        /// <summary>
        /// Id convention.
        /// </summary>
        public enum Placing
        {
            /// <summary>
            /// Do not change.
            /// </summary>
            DoNotChange,

            /// <summary>
            /// Id at begining.
            /// </summary>
            Prefix,

            /// <summary>
            /// Id at the end.
            /// </summary>
            Sufix
        }

        /// <summary>
        /// Naming case.
        /// </summary>
        public enum Case
        {
            /// <summary>
            /// Do not change.
            /// </summary>
            DoNotChange,

            /// <summary>
            /// Camel case (thisIsAnExample).
            /// </summary>
            Camel,

            /// <summary>
            /// Pascal case (ThisIsAnExample).
            /// </summary>
            Pascal,

            /// <summary>
            /// Snake case (this_is_an_example).
            /// </summary>
            Snake,

            /// <summary>
            /// Upper snake case (THIS_IS_AN_EXAMPLE).
            /// </summary>
            UpperSnake,

            /// <summary>
            /// Kebab case (this-is-an-example).
            /// </summary>
            Kebab,

            /// <summary>
            /// Upper kebab case (THIS-IS-AN-EXAMPLE).
            /// </summary>
            UpperKebab
        }

        #endregion

        /// <summary>
        /// Id text.
        /// </summary>
        public string IdText;

        /// <summary>
        /// Source of text.
        /// </summary>
        public Source TextSource;

        /// <summary>
        /// Scope of translation.
        /// </summary>
        public Translation Scope;

        /// <summary>
        /// Id mode.
        /// </summary>
        public Placing IdPlacing;

        /// <summary>
        /// Naming case for stored procedures parameters.
        /// </summary>
        public Case ParametersCase;

        /// <summary>
        /// Naming case for stored procedures.
        /// </summary>
        public Case StoredProceduresCase;

        /// <summary>
        /// Naming case for table fields.
        /// </summary>
        public Case FieldsCase;

        /// <summary>
        /// Naming case for tables.
        /// </summary>
        public Case TablesCase;

        #region Constructors

        public DbNaming()
        {
        }

        public DbNaming(Placing idPlacing, Case namingCase, string idText = null)
        {
            TextSource = Source.Default;
            Scope = Translation.Reflection;
            IdPlacing = idPlacing;
            ParametersCase = StoredProceduresCase = FieldsCase = TablesCase = namingCase;
            IdText = idText;
        }

        #endregion

        /// <summary>
        /// Gets a parameter name.
        /// </summary>
        public virtual string GetParameterName(PropertyInfo propInfo, string parameterName)
        {
            return TranslateName(TextSource, Scope, propInfo.Name, parameterName, ParametersCase, IdPlacing, IdText);
        }

        /// <summary>
        /// Gets a field name.
        /// </summary>
        public virtual string GetFieldName(PropertyInfo propInfo, string fieldName)
        {
            return TranslateName(TextSource, Scope, propInfo.Name, fieldName, FieldsCase, IdPlacing, IdText);
        }

        /// <summary>
        /// Translates a name.
        /// </summary>
        public static string TranslateName(Source source, Translation scope, string reflectionName, string configurationName, Case namingCase, Placing idPlacing, string idText)
        {
            string name;

            switch (source)
            {
                case Source.Default:
                default:
                    if (!string.IsNullOrEmpty(configurationName))
                    {
                        source = Source.Configuration;
                        name = configurationName;
                    }
                    else
                    {
                        source = Source.Reflection;
                        name = reflectionName;
                    }
                    break;

                case Source.Configuration:
                    name = configurationName;
                    break;

                case Source.Reflection:
                    name = reflectionName;
                    break;
            }

            if (scope == Translation.All
                || scope == Translation.Configuration && source == Source.Configuration
                || scope == Translation.Reflection && source == Source.Reflection)
            {
                name = Translate(name, namingCase, idPlacing, idText);
            }

            return name;
        }

        /// <summary>
        /// Translates a name.
        /// </summary>
        public static string Translate(string name, Case namingCase, Placing idPlacing, string idText)
        {
            var words = SplitInWords(name, namingCase);
            var count = words.Length;

            var startIndex = 0;
            if (count > 1 && !string.IsNullOrEmpty(idText))
            {
                switch (idPlacing)
                {
                    case Placing.DoNotChange:
                    default:
                        break;

                    case Placing.Prefix:
                        if (string.Compare(words[0], idText, true) != 0 && string.Compare(words[count - 1], idText, true) == 0)
                        {
                            startIndex = count - 1;
                            words[startIndex] = idText;
                        }
                        break;

                    case Placing.Sufix:
                        if (string.Compare(words[0], idText, true) == 0 && string.Compare(words[count - 1], idText, true) != 0)
                        {
                            startIndex = 1;
                            words[0] = idText;
                        }
                        break;
                }
            }

            var separator = GetSeparator(namingCase);

            var result = new StringBuilder();

            for (var index = 0; index < count; index++)
            {
                var word = words[(index + startIndex) % count];

                if (index > 0)
                {
                    result.Append(separator);
                }

                result.Append(word);
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

                    case Case.DoNotChange:
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

                case Case.DoNotChange:
                case Case.Camel:
                case Case.Pascal:
                default:
                    return string.Empty;
            }
        }
    }
}
