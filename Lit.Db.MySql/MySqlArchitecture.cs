using System;
using Lit.Db.Framework;
using Lit.Db.Model;
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
            new CreateTable(tableTemplate, Setup).Exec(this);
        }

        /// <summary>
        /// Drop a table.
        /// </summary>
        public void DropTable(Type tableTemplate, bool onlyIfExists)
        {
            new DropTable(tableTemplate, Setup, onlyIfExists).Exec(this);
        }

        /// <summary>
        /// Create a stored procedure.
        /// </summary>
        public void CreateStoredProcedure(Type tableTemplate, StoredProcedureFunction spFunc)
        {
            switch (spFunc)
            {
                case StoredProcedureFunction.Get:
                case StoredProcedureFunction.Find:
                    new CreateStoredProcedureGet(tableTemplate, Setup, spFunc).Exec(this);
                    return;

                case StoredProcedureFunction.ListAll:
                    new CreateStoredProcedureListAll(tableTemplate, Setup).Exec(this);
                    return;

                case StoredProcedureFunction.Insert:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureInsert(tableTemplate, Setup).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureInsertGet(tableTemplate, Setup).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Update:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureUpdate(tableTemplate, Setup).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureUpdateGet(tableTemplate, Setup).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Store:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureStore(tableTemplate, Setup).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureStoreGet(tableTemplate, Setup).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Delete:
                    new CreateStoredProcedureDelete(tableTemplate, Setup).Exec(this);
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
            new DropStoredProcedure(tableTemplate, Setup, spFunc, onlyIfExists).Exec(this);
        }
    }
}
