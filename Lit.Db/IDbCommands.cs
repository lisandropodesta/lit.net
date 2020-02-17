using System;

namespace Lit.Db
{
    /// <summary>
    /// Commands interface with a Db.
    /// </summary>
    public interface IDbCommands
    {
        /// <summary>
        /// Naming manager.
        /// </summary>
        IDbNaming DbNaming { get; }

        /// <summary>
        /// Executes a query template.
        /// </summary>
        T ExecuteQuery<T>(string query, Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Executes a query template.
        /// </summary>
        T ExecuteQuery<T>(Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        T ExecuteStoredProcedure<T>(string storedProcedureName, Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Execute a stored procedure template.
        /// </summary>
        void ExecuteStoredProcedure<T>(string storedProcedureName, T template);

        /// <summary>
        /// Execute a stored procedure/query template.
        /// </summary>
        T ExecuteTemplate<T>(Action<T> setup = null)
            where T : new();

        /// <summary>
        /// Execute a stored procedure/query template.
        /// </summary>
        void ExecuteTemplate<T>(T template);
    }
}
