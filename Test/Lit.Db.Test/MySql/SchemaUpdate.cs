using Lit.Auditing;
using Lit.Db.MySql.Schema.Information;
using Lit.Db.MySql.Statements;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.MySql
{
    public static class SchemaUpdate
    {
        public static void Execute(IDbHost db)
        {
            Audit.Message("\n  ** DROP TABLEs test **");
            new DropTable(typeof(UserSession), db.DbNaming, true).Exec(db);
            new DropTable(typeof(User), db.DbNaming, true).Exec(db);

            Audit.Message("\n  ** CREATE TABLEs test **");
            new CreateTable(typeof(User), db.DbNaming, Engine.InnoDb, "latin1").Exec(db);
            new CreateTable(typeof(UserSession), db.DbNaming, Engine.InnoDb, "latin1").Exec(db);
        }
    }
}
