using System;
using System.Collections.Generic;
using System.Linq;
using Lit.Names;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Default naming convention for Sql.
    /// </summary>
    public class SqlDefaultNaming : DbNaming
    {
        public SqlDefaultNaming() : base(AffixPlacing.Prefix, Case.Pascal, "Id")
        {
            ForceIdOnKeyColumn = true;
        }

        public override string GetSqlType(DbDataType dataType, Type type = null, ulong? size = null, int? precision = null)
        {
            throw new NotImplementedException();
        }

        protected override string[] SplitParts(string compoundName)
        {
            var length = compoundName.Length;
            var strings = new string[0];
            var word = string.Empty;
            var invalid = length == 0;
            bool bracesStarted = false;
            bool bracesEnded = false;

            for (var i = 0; !invalid && i <= length; i++)
            {
                var flushWord = false;

                if (i >= length)
                {
                    if (compoundName[length - 1] == '.')
                    {
                        invalid = true;
                    }
                    else if (!bracesEnded)
                    {
                        flushWord = true;
                    }
                }
                else
                {
                    var c = compoundName[i];
                    if (bracesEnded)
                    {
                        if (c != '.')
                        {
                            invalid = true;
                        }

                        bracesStarted = false;
                        bracesEnded = false;
                    }
                    else if (bracesStarted)
                    {
                        if (c == ']')
                        {
                            bracesEnded = true;
                            flushWord = true;
                        }
                        else
                        {
                            word += c;
                        }
                    }
                    else if (c == '[')
                    {
                        bracesStarted = true;
                    }
                    else if (c == '.')
                    {
                        flushWord = true;
                    }
                    else
                    {
                        word += c;
                    }
                }

                if (flushWord && !invalid)
                {
                    if (word.Length > 0)
                    {
                        Array.Resize(ref strings, strings.Length + 1);
                        strings[strings.Length - 1] = word;
                        word = string.Empty;
                    }
                    else
                    {
                        invalid = true;
                    }
                }
            }

            if (invalid)
            {
                throw new ArgumentException($"Invalid name '{compoundName}'");
            }

            return strings;
        }

        protected override string JoinParts(IEnumerable<string> parts)
        {
            return string.Join(".", parts.Select(n => $"[{n}]"));
        }
    }
}
