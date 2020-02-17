using System;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    class Program
    {
        private static string wikialgorithmConnectionStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";

        static void Main(string[] args)
        {
            Console.WriteLine("Lit.Db.Test");

            try
            {
                Console.WriteLine("\n\n*** NAMING TEST ***");
                Common.Naming.Execute();

                var db = new MySqlHost(wikialgorithmConnectionStr)
                {
                    DbNaming = new MySqlDefaultNaming()
                };

                Console.WriteLine("\n\n*** STORED PROCEDURE TEST ***");
                MySql.StoredProcedure.Execute(db);

                Console.WriteLine("\n\n*** INFORMATION SCHEMA TEST ***");
                MySql.InformationSchema.Load(db);
            }
            catch (Exception x)
            {
                Console.WriteLine($"Error: {x.Message}");
            }
        }

    }
}
