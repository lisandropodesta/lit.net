using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Lit.Db.Attributes;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    class Program
    {
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

        static void Main(string[] args)
        {
            Console.WriteLine("Lit.Db.Test");

            try
            {
                //NamingTest();

                string connStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";
                var conn = new MySqlConnection(connStr);
                conn.Open();

                var db = new MySqlHost("server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing")
                {
                    DbNaming = new MySqlDefaultNaming()
                };

                var data = db.ExecuteTemplate<Test>(p =>
                {
                    p.IntegerValue = 3;
                });

                MySqlTest.Execute(db);
            }
            catch (Exception x)
            {
                Console.WriteLine($"Error: {x.Message}");
            }
        }

        private class NamingExample
        {
            public DbNaming.Placing Id;

            public DbNaming.Case Case;

            public string Text;

            public string ExpectedResult;
        }

        private static readonly NamingExample[] namingList = new NamingExample[]
        {
            // Snake case
            new NamingExample { Id = DbNaming.Placing.Sufix, Case = DbNaming.Case.Snake, Text = "idABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new NamingExample { Id = DbNaming.Placing.Sufix, Case = DbNaming.Case.Snake, Text = "id__ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x_id" },

            // Snake case
            new NamingExample { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Snake, Text = "ID  ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "id_abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Snake case
            new NamingExample { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Snake, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc_def_g_hi_jkl_mno_pqr123_stu456_vw_x" },

            // Kebab case
            new NamingExample { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Kebab, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abc-def-g-hi-jkl-mno-pqr123-stu456-vw-x" },

            // Camel case
            new NamingExample { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Camel, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "abcDefGHiJklMnoPqr123Stu456VwX" },

            // Pascal case
            new NamingExample { Id = DbNaming.Placing.Prefix, Case = DbNaming.Case.Pascal, Text = "ABC DEF_GHi,jklMno+PQR123stu456VwX", ExpectedResult = "AbcDefGHiJklMnoPqr123Stu456VwX" },
        };

        private static void NamingTest()
        {
            foreach (var i in namingList)
            {
                var text = DbNaming.Translate(i.Text, i.Case, i.Id, "id");
                if (text != i.ExpectedResult)
                {
                    throw new Exception("Wrong translation");
                }
            }
        }
    }
}
