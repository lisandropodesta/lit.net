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

        public MySqlArchitecture(string connectionString, bool useReadAfterWrite = true)
            : base(connectionString)
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
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureInsert(tableTemplate, DbNaming).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureInsertGet(tableTemplate, DbNaming).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Update:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureUpdate(tableTemplate, DbNaming).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureUpdateGet(tableTemplate, DbNaming).Exec(this);
                    }
                    return;

                case StoredProcedureFunction.Set:
                    if (!useReadAfterWrite)
                    {
                        new CreateStoredProcedureSet(tableTemplate, DbNaming).Exec(this);
                    }
                    else
                    {
                        new CreateStoredProcedureSetGet(tableTemplate, DbNaming).Exec(this);
                    }
                    return;

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
