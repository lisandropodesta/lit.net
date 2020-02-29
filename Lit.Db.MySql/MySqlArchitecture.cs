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
                case StoredProcedureFunction.GetByCode:
                    new CreateStoredProcedureGet(tableTemplate, DbNaming, spFunc).Exec(this);
                    return;

                case StoredProcedureFunction.ListAll:
                    new CreateStoredProcedureListAll(tableTemplate, DbNaming).Exec(this);
                    return;

                case StoredProcedureFunction.Insert:
                    new CreateStoredProcedureInsert(tableTemplate, DbNaming).Exec(this);
                    return;

                case StoredProcedureFunction.InsertGet:
                    new CreateStoredProcedureInsertGet(tableTemplate, DbNaming).Exec(this);
                    return;

                case StoredProcedureFunction.Update:
                case StoredProcedureFunction.Set:
                    break;

                case StoredProcedureFunction.Delete:
                    new CreateStoredProcedureDelete(tableTemplate, DbNaming).Exec(this);
                    return;

                default:
                    break;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Drop a stored procedure.
        /// </summary>
        public void DropStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc, bool onlyIfExists = false)
        {
            new DropStoredProcedure(tableTemplate, DbNaming, spFunc, onlyIfExists).Exec(this);
        }
    }
}
