namespace Lit.Db
{
    /// <summary>
    /// Database query/stored procedure template.
    /// </summary>
    public class DbTemplate
    {
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
