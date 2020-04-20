using MySql.Data.MySqlClient;
using Lit.Db.Framework.Entities;

namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql data access.
    /// </summary>
    public class MySqlDataAccess : DbDataAccess<MySqlConnection, MySqlCommand>
    {
        #region Constructors

        public MySqlDataAccess(IDbSetup setup, string connectionString) : base(setup, connectionString) { }

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
