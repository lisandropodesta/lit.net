using Lit.Db.Custom.MySql;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable(Engine.Memory, "latin1")]
    public class StatusConnection : DbRecordBase
    {
        [DbPrimaryKey]
        public int IdConnection { get; protected set; }

        [DbForeignKey(typeof(UserSession))]
        public int IdUserSession { get; set; }

        [DbUniqueKey(IsNullable = false)]
        public string Token { get; set; }
    }
}
