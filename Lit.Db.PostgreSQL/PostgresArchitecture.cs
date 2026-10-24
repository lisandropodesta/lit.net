using System;
using Lit.Db.Framework;
using Lit.Db.PostgreSQL.Statements;

namespace Lit.Db.PostgreSQL
{
    /// <summary>
    /// PostgreSQL db architecture access.
    /// </summary>
    public class PostgresArchitecture : PostgresDataAccess, IDbArchitecture
    {
        #region Constructors

        public PostgresArchitecture(IDbSetup setup, string connectionString, bool useReadAfterWrite = true)
            : base(setup, connectionString)
        {
            this.useReadAfterWrite = useReadAfterWrite;
        }

        #endregion

        private bool useReadAfterWrite;

        /// <summary>
        /// Create a table.
        /// </summary>
        public void CreateTable(Type tableTemplate)
        {
            new CreateTable(Setup, tableTemplate).Exec(this);
        }

        /// <summary>
        /// Drop a table.
        /// </summary>
        public void DropTable(Type tableTemplate, bool onlyIfExists)
        {
            new DropTable(Setup, tableTemplate, onlyIfExists).Exec(this);
        }

        /// <summary>
        /// Create a stored procedure.
        /// </summary>
        public void CreateStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc)
        {
            // PostgreSQL uses functions instead of stored procedures
            // Implementation can be added in future versions
            throw new NotImplementedException("Stored procedures are not yet implemented for PostgreSQL. Consider using functions instead.");
        }

        /// <summary>
        /// Drop a stored procedure.
        /// </summary>
        public void DropStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc, bool onlyIfExists = false)
        {
            // PostgreSQL uses functions instead of stored procedures
            // Implementation can be added in future versions
            throw new NotImplementedException("Stored procedures are not yet implemented for PostgreSQL. Consider using functions instead.");
        }
    }
}
