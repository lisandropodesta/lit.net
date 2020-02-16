using System.Data.SqlClient;
using Lit.Db.Model;
using Lit.Db.Sql.Model;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Sql host.
    /// </summary>
    public class SqlHost : DbHost<SqlConnection, SqlCommand>
    {
        #region Constructors

        public SqlHost(string connectionString) : base(connectionString) { }

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
