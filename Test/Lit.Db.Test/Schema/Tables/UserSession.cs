using System;
using System.Collections.Generic;
using Lit.Db.Custom.MySql;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable("latin1")]
    public class UserSession : DbRecordBase
    {
        [DbPrimaryKey]
        public long IdUserSession { get; set; }

        [DbForeignKey(typeof(UserSession))]
        public long? IdPrevUserSession { get; set; }

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

        [DbColumn(16777215)]
        public string LongText { get; set; }

        public UserSession() { }

        public UserSession(long id)
        {
            IdUserSession = id;
        }
    }
}
