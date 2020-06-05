using System.Text;

namespace Lit.Db
{
    /// <summary>
    /// Database query/stored procedure template.
    /// </summary>
    public class DbTemplate
    {
        protected IDbSetup Setup;

        #region Constructor

        public DbTemplate(IDbSetup setup)
        {
            Setup = setup;
        }

        #endregion
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

        /// <summary>
        /// Append a separated text.
        /// </summary>
        public static void ConditionalAppend(this StringBuilder instance, string separator, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (instance.Length > 0)
                {
                    instance.Append(separator);
                }

                instance.Append(text);
            }
        }
    }
}
