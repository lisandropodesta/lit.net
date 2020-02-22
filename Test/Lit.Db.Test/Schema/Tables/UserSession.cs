using System;
using Lit.Db.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
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
