using System;
using Lit.Db.Architecture;
using Lit.Db.MySql.Statements;
using Lit.Db.MySql.Statements.Queries;

namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql db architecture access.
    /// </summary>
    public class MySqlArchitecture : MySqlHost, IDbArchitecture
    {
        #region Constructors

        public MySqlArchitecture(string connectionString) : base(connectionString) { }

        #endregion

        /// <summary>
        /// Create a table.
        /// </summary>
        public void CreateTable(Type tableTemplate)
        {
            new CreateTable(tableTemplate, DbNaming).Exec(this);
        }

        /// <summary>
        /// Drop a table.
        /// </summary>
        public void DropTable(Type tableTemplate, bool onlyIfExists)
        {
            new DropTable(tableTemplate, DbNaming, onlyIfExists).Exec(this);
        }

        /// <summary>
        /// Create a stored procedure.
        /// </summary>
        public void CreateStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc)
        {
            switch (spFunc)
            {
                case StoredProcedureFunction.Get:
                    new CreateStoredProcedureGet(tableTemplate, DbNaming).Exec(this);
                    return;

                case StoredProcedureFunction.GetByCode:
                case StoredProcedureFunction.ListAll:
                case StoredProcedureFunction.Insert:
                case StoredProcedureFunction.Update:
                case StoredProcedureFunction.Set:
                case StoredProcedureFunction.Delete:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Drop a stored procedure.
        /// </summary>
        public void DropStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc, bool onlyIfExists)
        {
            throw new NotImplementedException();
        }
    }
}
