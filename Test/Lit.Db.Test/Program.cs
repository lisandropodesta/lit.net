using System;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    class Program
    {
        private static string wikialgorithmConnectionStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";

        private static string testingConnectionStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";

        private static IDbNaming naming = new MySqlDefaultNaming();

        static void Main(string[] args)
        {
            Console.WriteLine("Lit.Db.Test");

            try
            {
                Console.WriteLine("\n\n*** NAMING TEST ***");
                var tdb = new MySqlHost(testingConnectionStr) { DbNaming = naming };
                MySql.SchemaUpdate.Execute(tdb);

                Console.WriteLine("\n\n*** NAMING TEST ***");
                Common.Naming.Execute();

                var db = new MySqlHost(wikialgorithmConnectionStr) { DbNaming = naming };

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
