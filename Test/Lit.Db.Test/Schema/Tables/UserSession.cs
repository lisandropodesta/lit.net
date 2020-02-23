using System;
using Lit.Db.Attributes;
using Lit.Db.Custom.MySql.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable("latin1")]
    public class UserSession
    {
        [DbPrimaryKey]
        public int IdUserSession { get; protected set; }

        [DbForeignKey(typeof(User), nameof(User.IdUser))]
        public int IdUser { get; protected set; }

        [DbColumn]
        public DateTimeOffset Started { get; protected set; }
    }
}
