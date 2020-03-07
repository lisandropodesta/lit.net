using Lit.Auditing;
using Lit.Db.Test.Schema;
using Lit.Db.Test.Schema.Tables;
using System;

namespace Lit.Db.Test.Common
{
    public static class DataAccess
    {
        public static void Execute(IDbDataAccess db)
        {
            Audit.Message("\n  ** USERSs test **");

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

            user = new User
            {
                FullName = "Mick Doe",
                NickName = "mck",
                Status = Status.Active
            };

            db.Set(user);
            Assert(user.IdUser > 0);

            user = new User(1);
            db.Get(user);
            Assert(user.FullName == "John Nash");

            user.NickName = "mck";
            db.GetByCode(user);
            Assert(user.FullName == "Mick Doe");

            var list = db.List<User>();
            foreach (var u in list)
            {
                Audit.Message($"  IdUser={u.IdUser}, Status={u.Status}, NickName={u.NickName}, FullName={u.FullName}");
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
