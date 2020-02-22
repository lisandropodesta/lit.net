using System.Text;

namespace Lit.Db
{
    /// <summary>
    /// Database query/stored procedure template.
    /// </summary>
    public class DbTemplate
    {
        /// <summary>
        /// Adds a space and a text to a string.
        /// </summary>
        protected static void AddText(StringBuilder str, string text, bool space = true)
        {
            if (!string.IsNullOrEmpty(text))
            {
                str.Append((space ? " " : string.Empty) + text);
            }
        }

        /// <summary>
        /// Adds a new line and a text to a string.
        /// </summary>
        protected static void AddTextLine(StringBuilder str, string text, bool tab = true)
        {
            if (!string.IsNullOrEmpty(text))
            {
                AddText(str, "\n" + (tab ? "  " : string.Empty) + text);
            }
        }
    }

    /// <summary>
    /// Extension methods for DbTemplate.
    /// </summary>
    public static class DbTemplateExtension
    {
        /// <summary>
        /// Executes the template.
        /// </summary>
        public static T Exec<T>(this T instance, IDbCommands db)
            where T : DbTemplate
        {
            db.ExecuteTemplate(instance);
            return instance;
        }
    }
}
