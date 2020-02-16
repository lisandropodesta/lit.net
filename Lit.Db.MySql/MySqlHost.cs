using MySql.Data.MySqlClient;
using Lit.Db.Model;
using Lit.Db.MySql.Model;

namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql host.
    /// </summary>
    public class MySqlHost : DbHost<MySqlConnection, MySqlCommand>
    {
        #region Constructors

        public MySqlHost(string connectionString) : base(connectionString) { }

        #endregion

        protected override MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        protected override DbStoredProcedure<MySqlConnection, MySqlCommand> CreateStoredProcedure(string name, MySqlConnection connection)
        {
            return new MySqlStoredProcedure(name, connection);
        }
    }
}
