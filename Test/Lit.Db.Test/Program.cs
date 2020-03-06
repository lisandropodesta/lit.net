using System;
using Lit.Auditing;
using Lit.Db.Framework;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    class Program
    {
        private static readonly string wikialgorithmConnectionStr = "server=localhost; port=3306; database=wikialgorithm; uid=lisandro; pwd=lisandro_testing";

        private static readonly string testingConnectionStr = "server=localhost; port=3306; database=testing; uid=lisandro; pwd=lisandro_testing";

        private static readonly IDbNaming naming = new MySqlDefaultNaming();

        private static readonly IDbHost db = new MySqlHost(wikialgorithmConnectionStr) { DbNaming = naming };

        private static readonly IDbArchitecture tdb = new MySqlArchitecture(testingConnectionStr) { DbNaming = naming };

        static void Main(string[] args)
        {
            NLogAudit.Register(AuditType.Debug, AuditTarget.Console, AuditTarget.File);

            Audit.Message("Lit.Db.Test");

            try
            {
                Audit.Message("\n\n*** NAMING TEST ***");
                Common.Naming.Execute();

                Audit.Message("\n\n*** STORED PROCEDURE TEST ***");
                MySql.StoredProcedure.Execute(db);

                Audit.Message("\n\n*** INFORMATION SCHEMA TEST ***");
                MySql.InformationSchema.Load(db);

                Audit.Message("\n\n*** SCHEMA UPDATE TEST ***");
                MySql.SchemaUpdate.Execute(tdb);
            }
            catch (Exception x)
            {
                Audit.Error($"Error: {x.Message}.");
            }
        }

    }
}
