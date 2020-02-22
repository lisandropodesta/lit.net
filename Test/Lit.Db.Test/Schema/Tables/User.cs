using Lit.Db.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    public class User
    {
        [DbPrimaryKey]
        public int IdUser { get; protected set; }

        [DbColumn]
        public string NickName { get; protected set; }
    }
}
