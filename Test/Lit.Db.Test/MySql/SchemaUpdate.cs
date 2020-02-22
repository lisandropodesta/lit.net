using System;
using Lit.Db.MySql.Schema.Information;
using Lit.Db.MySql.Statements;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.MySql
{
    public static class SchemaUpdate
    {
        public static void Execute(IDbHost db)
        {
            Console.WriteLine("\n  ** DROP TABLE test **");
            new DropTable(typeof(User), db.DbNaming).Exec(db);

            Console.WriteLine("\n  ** CREATE TABLE test **");
            var query = new CreateTable(typeof(User), db.DbNaming, Engine.InnoDb, "latin1").Exec(db);
        }
    }
}
