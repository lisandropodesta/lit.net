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

        protected override MySqlCommand CreateCommand(string name, MySqlConnection connection)
        {
            return new MySqlCommand(name, connection);
        }

        protected override DbStoredProcedure<MySqlCommand> CreateStoredProcedure(string name, MySqlCommand command)
        {
            return new MySqlStoredProcedure(name, command);
        }
    }
}
