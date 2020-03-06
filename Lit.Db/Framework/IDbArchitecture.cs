using System;

namespace Lit.Db.Framework
{
    /// <summary>
    /// Db architecture access.
    /// </summary>
    public interface IDbArchitecture : IDbHost
    {
        /// <summary>
        /// Create a table.
        /// </summary>
        void CreateTable(Type tableTemplate);

        /// <summary>
        /// Drop a table.
        /// </summary>
        void DropTable(Type tableTemplate, bool onlyIfExists);

        /// <summary>
        /// Create a table stored procedure.
        /// </summary>
        void CreateStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc);

        /// <summary>
        /// Drop a stored procedure.
        /// </summary>
        void DropStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc, bool onlyIfExists);
    }
}
