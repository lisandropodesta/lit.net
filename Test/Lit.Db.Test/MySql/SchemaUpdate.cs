using Lit.Db.MySql;
using Lit.Db.MySql.Schema.Information;
using Lit.Db.MySql.Statements.Queries;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.MySql
{
    public static class SchemaUpdate
    {
        public static void Execute(MySqlHost db)
        {
            var query = new CreateTable(db, Engine.InnoDb, "test", "latin1", typeof(User));
        }
    }
}
