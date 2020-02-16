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

        protected override SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected override DbStoredProcedure<SqlConnection, SqlCommand> CreateStoredProcedure(string name, SqlConnection connection)
        {
            return new SqlStoredProcedure(name, connection);
        }
    }
}
