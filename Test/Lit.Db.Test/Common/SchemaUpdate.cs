using System;
using Lit.Auditing;
using Lit.Db.Framework;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.Common
{
    public static class SchemaUpdate
    {
        public static void Execute(IDbArchitecture db)
        {
            Audit.Message("\n  ** DROP TABLEs test **");
            db.DropTable(typeof(Folder), true);
            db.DropTable(typeof(Tag), true);
            db.DropTable(typeof(TagCategory), true);
            db.DropTable(typeof(UserSession), true);
            db.DropTable(typeof(User), true);
            db.DropTable(typeof(StatusConnection), true);

            Audit.Message("\n  ** CREATE TABLEs test **");
            db.CreateTable(typeof(User));
            db.CreateTable(typeof(UserSession));
            db.CreateTable(typeof(StatusConnection));
            db.CreateTable(typeof(Folder));
            db.CreateTable(typeof(TagCategory));
            db.CreateTable(typeof(Tag));

            Audit.Message("\n  ** CREATE STORED PROCEDURES test **");
            CreateAllStoredProcedures(db, typeof(Folder));
            CreateAllStoredProcedures(db, typeof(Tag));
            CreateAllStoredProcedures(db, typeof(TagCategory));
            CreateAllStoredProcedures(db, typeof(User));
            CreateAllStoredProcedures(db, typeof(UserSession));
            CreateAllStoredProcedures(db, typeof(StatusConnection));
        }

        private static void CreateAllStoredProcedures(IDbArchitecture db, Type tableTemplate)
        {
            Audit.Message($"\n    ** Table [{tableTemplate.Name}] **");

            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Get);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Find);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Insert);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Update);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Store);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.ListAll);
            CreateStoredProcedure(db, tableTemplate, StoredProcedureFunction.Delete);
        }

        private static void CreateStoredProcedure(IDbArchitecture db, Type tableTemplate, StoredProcedureFunction function)
        {
            Audit.Message($"\n    ** Creating table function [{function}] **");

            db.DropStoredProcedure(tableTemplate, function, true);

            if (function == StoredProcedureFunction.Find && db.Setup.GetTableBinding(tableTemplate).SingleColumnUniqueKey == null)
            {
                Audit.Message($"\n    SKIPPED");
            }
            else
            {
                db.CreateStoredProcedure(tableTemplate, function);
            }
        }
    }
}
