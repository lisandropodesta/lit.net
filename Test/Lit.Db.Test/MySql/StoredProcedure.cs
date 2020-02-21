using System;
using System.Collections.Generic;
using Lit.Db.Attributes;

namespace Lit.Db.Test.MySql
{
    public static class StoredProcedure
    {
        public static void Execute(IDbHost db)
        {
            var data = db.ExecuteTemplate<Test>(p =>
            {
                p.IntegerValue = 3;
            });

            Console.WriteLine("\n  ** ALGORITHMS **");
            foreach (var r in data.Algorithms)
            {
                Console.WriteLine($"  {r.IdAlgorithm}, {r.Name}, {r.InvalidField}");
            }

            Console.WriteLine("\n  ** USERS **");
            foreach (var r in data.Users)
            {
                Console.WriteLine($"  {r.IdUser}, {r.Name}");
            }
        }

        protected class Algorithm
        {
            [DbField]
            public int IdAlgorithm { get; protected set; }

            [DbField]
            public string Name { get; protected set; }

            [DbField(isOptional: true)]
            public string InvalidField { get; protected set; }
        }

        protected class User
        {
            [DbField]
            public int IdUser { get; protected set; }

            [DbField]
            public string Name { get; protected set; }
        }

        [DbStoredProcedure("test")]
        protected class Test
        {
            [DbParameter]
            public int IntegerValue { get; set; }

            [DbRecordset(0)]
            public List<Algorithm> Algorithms { get; protected set; }

            [DbRecordset(1)]
            public List<User> Users { get; protected set; }
        }
    }
}
