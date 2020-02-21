using System;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    class Program
    {
        private static readonly string wikialgorithmConnectionStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";

        private static readonly string testingConnectionStr = "server=localhost; port=3306; database=testing; uid=lisandro; pwd=lisandro_testing";

        private static readonly IDbNaming naming = new MySqlDefaultNaming();

        private static readonly IDbHost db = new MySqlHost(wikialgorithmConnectionStr) { DbNaming = naming };

        private static readonly IDbHost tdb = new MySqlHost(testingConnectionStr) { DbNaming = naming };

        static void Main(string[] args)
        {
            Console.WriteLine("Lit.Db.Test");

            try
            {
                Console.WriteLine("\n\n*** NAMING TEST ***");
                Common.Naming.Execute();

                Console.WriteLine("\n\n*** STORED PROCEDURE TEST ***");
                MySql.StoredProcedure.Execute(db);

                Console.WriteLine("\n\n*** INFORMATION SCHEMA TEST ***");
                MySql.InformationSchema.Load(db);

                Console.WriteLine("\n\n*** SCHEMA UPDATE TEST ***");
                MySql.SchemaUpdate.Execute(tdb);
            }
            catch (Exception x)
            {
                Console.WriteLine($"Error: {x.Message}");
            }
        }

    }
}
