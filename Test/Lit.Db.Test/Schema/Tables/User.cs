using Lit.Db.Custom.MySql;
using Lit.Db.Framework.Entities;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable("latin1")]
    public class User : DbRecordBase, IDbStringCode
    {
        [DbPrimaryKey]
        public int IdUser { get; set; }

        [DbColumn(IsNullable = true)]
        public IDbForeignKeyRef<User> RefereeUser { get; set; }

        [DbColumn]
        public Status Status { get; set; }

        [DbUniqueKey]
        public string NickName { get; set; }

        [DbColumn]
        public string FullName { get; set; }

        [DbColumn(IsNullable = true)]
        public string Address { get; set; }

        string IDbStringCode.Code { get => NickName; set => NickName = value; }

        public User() { }

        public User(int id)
        {
            IdUser = id;
        }
    }
}
