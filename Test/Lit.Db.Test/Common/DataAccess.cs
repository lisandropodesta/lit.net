using System;
using System.Collections.Generic;
using Lit.Auditing;
using Lit.Db.Test.Schema;
using Lit.Db.Test.Schema.Tables;

namespace Lit.Db.Test.Common
{
    public static class DataAccess
    {
        public static void Execute(IDbDataAccess db)
        {
            Audit.Message("\n  ** User table test **");

            var user = new User
            {
                FullName = "John Nash",
                NickName = "John",
                Status = Status.Active
            };

            db.Insert(user);
            Assert(user.IdUser > 0);

            user.NickName = "j0hn";
            user.Status = Status.Hold;
            db.Update(user);
            var idFirstUser = user.IdUser;

            user = new User
            {
                FullName = "Mick Doe",
                NickName = "mck",
                RefereeUser = DbHelper.CreateKey<User>(db, idFirstUser, true),
                Status = Status.Active
            };

            Assert(user.RefereeUser.Record.IdUser == idFirstUser);

            db.Store(user);
            Assert(user.IdUser > 0);

            user = new User(1);
            db.Get(user);
            Assert(user.FullName == "John Nash");

            user = db.Find<User>("not exists");
            Assert(user == null);

            user = db.Find<User>("mck");
            Assert(user.FullName == "Mick Doe");

            var userList = db.List<User>();
            foreach (var u in userList)
            {
                Audit.Message($"  IdUser={u.IdUser}, Status={u.Status}, NickName={u.NickName}, FullName={u.FullName}, IdRefereeUser={u.RefereeUser}");
            }


            Audit.Message("\n  ** UserSession table test **");

            var userSession = new UserSession
            {
                IdUser = userList[0].IdUser,
                Started = DateTimeOffset.Now,
                DateTime = DateTime.Now,
                TimeSpan = TimeSpan.FromTicks(1234567890),
                Attributes = new Dictionary<string, object>
                {
                    { "Number", 123 },
                    { "Text", "hello" },
                    { "Bool", true },
                    { "Null", null },
                    { "Array", new[] { 1, 2, 3 } }
                },
                LongText = "long text!"
            };

            db.Store(userSession);
            Assert(userSession.IdUserSession > 0);

            var userSession2 = new UserSession(userSession.IdUserSession);
            db.Get(userSession2);
            Assert(userSession.IdUser == userSession2.IdUser);

            var sessionList = db.List<UserSession>();
            foreach (var s in sessionList)
            {
                Audit.Message($"  IdUserSession={s.IdUserSession}, IdUser={s.IdUser}, Started={s.Started}, DateTime={s.DateTime}, TimeSpan={s.TimeSpan}");
            }
        }

        public static void Assert(bool validation, string description = null)
        {
            if (!validation)
            {
                throw new Exception($"Assertion failed [{description}].");
            }
        }
    }
}
