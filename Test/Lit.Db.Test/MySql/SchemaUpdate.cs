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

            CreateAllStoredProcedures(db, typeof(User));
            CreateAllStoredProcedures(db, typeof(UserSession));
        }

        private static void CreateAllStoredProcedures(IDbArchitecture db, Type tableTemplate)
        {
            db.CreateStoredProcedure(tableTemplate, StoredProcedureFunction.Get);
        }
    }
}
