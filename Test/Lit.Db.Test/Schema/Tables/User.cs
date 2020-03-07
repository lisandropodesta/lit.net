using Lit.Db.Attributes;
using Lit.Db.Custom.MySql.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable("latin1")]
    public class User
    {
        [DbPrimaryKey]
        public int IdUser { get; protected set; }

        [DbColumn]
        public Status Status { get; set; }

        [DbUniqueKey]
        public string NickName { get; set; }

        [DbColumn]
        public string FullName { get; set; }

        [DbColumn(IsNullable = true)]
        public string Address { get; set; }

        public User() { }

        public User(int id)
        {
            IdUser = id;
        }
    }
}
