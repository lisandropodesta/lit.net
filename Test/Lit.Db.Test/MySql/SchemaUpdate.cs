using System;
using Lit.Auditing;
using Lit.Db.Architecture;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.MySql
{
    public static class SchemaUpdate
    {
        public static void Execute(IDbArchitecture db)
        {
            Audit.Message("\n  ** DROP TABLEs test **");
            db.DropTable(typeof(UserSession), true);
            db.DropTable(typeof(User), true);

            Audit.Message("\n  ** CREATE TABLEs test **");
            db.CreateTable(typeof(User));
            db.CreateTable(typeof(UserSession));

            Audit.Message("\n  ** CREATE STORED PROCEDURES test **");
            CreateAllStoredProcedures(db, typeof(User));
            CreateAllStoredProcedures(db, typeof(UserSession));
        }

        private static void CreateAllStoredProcedures(IDbArchitecture db, Type tableTemplate)
        {
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Get);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.GetByCode);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.InsertGet);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.UpdateGet);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.ListAll);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Delete);
        }

        private static void CreateStoredProcedure(IDbArchitecture db, Type tableTemplate, StoredProcedureFunction function)
        {
            Audit.Message($"\n   ** Creating table function [{function}] **");

            db.DropStoredProcedure(tableTemplate, function, true);

            try
            {
                db.CreateStoredProcedure(tableTemplate, function);
            }
            catch (Exception x)
            {
                Audit.Error($"Error: {x.Message}.");
            }
        }
    }
}
