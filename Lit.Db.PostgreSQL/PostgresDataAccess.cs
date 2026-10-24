using Npgsql;
using Lit.Db.Framework.Entities;

namespace Lit.Db.PostgreSQL
{
    /// <summary>
    /// PostgreSQL data access.
    /// </summary>
    public class PostgresDataAccess : DbDataAccess<NpgsqlConnection, NpgsqlCommand>
    {
        #region Constructors

        public PostgresDataAccess(IDbSetup setup, string connectionString) : base(setup, connectionString) { }

        #endregion

        protected override NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        protected override NpgsqlCommand CreateCommand(string name, NpgsqlConnection connection)
        {
            return new NpgsqlCommand(name, connection);
        }

        protected override DbStoredProcedure<NpgsqlCommand> CreateStoredProcedure(string name, NpgsqlCommand command)
        {
            return new PostgresStoredProcedure(name, command);
        }
    }
}
