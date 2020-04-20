using System;
using Lit.Auditing;
using Lit.Db.Framework;
using Lit.Db.MySql;
using Lit.Db.Test.Common;

namespace Lit.Db.Test
{
    class Program
    {
        private static readonly string testingConnectionStr = "server=localhost; port=3306; database=litdb-testing; uid=lisandro; pwd=lisandro_testing";

        private static readonly IDbNaming naming = new MySqlDefaultNaming();

        private static readonly IDbTranslation translation = new MySqlDefaultTranslation();

        private static readonly IDbSetup setup = new DbSetup(naming, translation);

        private static readonly IDbDataAccess db = new MySqlDataAccess(setup, testingConnectionStr);

        private static readonly IDbArchitecture tdb = new MySqlArchitecture(setup, testingConnectionStr);

        static void Main(string[] args)
        {
            NLogAudit.Register(AuditType.Debug, AuditTarget.Console, AuditTarget.File);

            Audit.Message("Lit.Db.Test");

            try
            {
                Audit.Message("\n\n*** NAMING TEST ***");
                Naming.Execute();

                //Audit.Message("\n\n*** STORED PROCEDURE TEST ***");
                //StoredProcedure.Execute(db);

                Audit.Message("\n\n*** INFORMATION SCHEMA TEST ***");
                MySql.InformationSchema.Load(db);

                Audit.Message("\n\n*** SCHEMA UPDATE TEST ***");
                SchemaUpdate.Execute(tdb);

                Audit.Message("\n\n*** DATA ACCESS TEST ***");
                DataAccess.Execute(db);

                Audit.Message("\n\n*** TEST FINISHED OK ***");
            }
            catch (Exception x)
            {
                Audit.Error($"Error: {x.Message}.");
            }
        }
    }
}
