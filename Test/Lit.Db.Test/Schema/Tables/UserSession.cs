using System;
using System.Collections.Generic;
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

        [DbForeignKey(typeof(User))]
        public int IdUser { get; set; }

        [DbColumn]
        public DateTimeOffset Started { get; set; }

        [DbColumn]
        public DateTimeOffset? Finished { get; set; }

        [DbColumn]
        public DateTime DateTime { get; set; }

        [DbColumn]
        public TimeSpan TimeSpan { get; set; }

        [DbColumn]
        public Dictionary<string, object> Attributes { get; set; }

        public UserSession() { }

        public UserSession(int id)
        {
            IdUserSession = id;
        }
    }
}
