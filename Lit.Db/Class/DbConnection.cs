using Lit.Db.Interface;
using System.Data.SqlClient;

namespace Lit.Db.Class
{
    /// <summary>
    /// Database connection.
    /// </summary>
    public class DbConnection : DbCommands, IDbConnection
    {
        /// <summary>
        /// Connection string name.
        /// </summary>
        public string ConnectionString => connectionString;

        private readonly string connectionString;

        #region Constructors

        public DbConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        /// <summary>
        /// Gets a new sql connection.
        /// </summary>
        protected override SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
