using Lit.Db.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    public class User
    {
        [DbPrimaryKey]
        public int IdUser { get; protected set; }

        [DbField]
        public string NickName { get; protected set; }
    }
}
