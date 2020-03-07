using System.Data.SqlClient;
using Lit.Db.Framework.Entities;
using Lit.Db.Model;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Sql data access.
    /// </summary>
    public class SqlDataAccess : DbDataAccess<SqlConnection, SqlCommand>
    {
        #region Constructors

        public SqlDataAccess(IDbSetup setup, string connectionString) : base(setup, connectionString) { }

        #endregion

        protected override SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected override SqlCommand CreateCommand(string name, SqlConnection connection)
        {
            return new SqlCommand(name, connection);
        }

        protected override DbStoredProcedure<SqlCommand> CreateStoredProcedure(string name, SqlCommand command)
        {
            return new SqlStoredProcedure(name, command);
        }
    }
}
