using System;
using Lit.Db.Framework;
using Lit.Db.MySql.Statements;
using Lit.Db.MySql.Statements.Queries;

namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql db architecture access.
    /// </summary>
    public class MySqlArchitecture : MySqlDataAccess, IDbArchitecture
    {
        #region Constructors

        public MySqlArchitecture(IDbSetup setup, string connectionString, bool useReadAfterWrite = true)
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
            switch (spFunc)
            {
                case StoredProcedureFunction.Get:
                    new CreateStoredProcedureGet(Setup, tableTemplate).Exec(this);
                    return;

                case StoredProcedureFunction.Find:
                    new CreateStoredProcedureFind(Setup, tableTemplate).Exec(this);
                    return;

                case StoredProcedureFunction.ListAll:
                    new CreateStoredProcedureListAll(Setup, tableTemplate).Exec(this);
                    return;

                case StoredProcedureFunction.Insert:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureInsert(Setup, tableTemplate).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureInsertGet(Setup, tableTemplate).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Update:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureUpdate(Setup, tableTemplate).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureUpdateGet(Setup, tableTemplate).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Store:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureStore(Setup, tableTemplate).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureStoreGet(Setup, tableTemplate).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Delete:
                    new CreateStoredProcedureDelete(Setup, tableTemplate).Exec(this);
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
            new DropStoredProcedure(Setup, tableTemplate, spFunc, onlyIfExists).Exec(this);
        }
    }
}
