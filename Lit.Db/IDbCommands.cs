using System;

namespace Lit.Db
{
    /// <summary>
    /// Commands interface with a Db.
    /// </summary>
    public interface IDbCommands
    {
        /// <summary>
        /// Executes a query loading results in a template.
        /// </summary>
        T ExecuteQuery<T>(string query)
            where T : new();

        /// <summary>
        /// Execute a stored procedure template with a parameters initialization action.
        /// </summary>
        T ExecuteTemplate<T>(Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Execute a stored procedure template with a parameters initialization action.
        /// </summary>
        T ExecuteTemplate<T>(string storedProcedureName, Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        void ExecuteTemplate<T>(T template);

        /// <summary>
        /// Execute a stored procedure template with parameters already initialized.
        /// </summary>
        void ExecuteTemplate<T>(T template, string storedProcedureName);
    }
}
