using System.Data;
using System.Data.SqlClient;

namespace Lit.Db.Class
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    public class DbStoredProcedure
    {
        private readonly string name;

        private readonly SqlParameter[] parameters;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DbStoredProcedure(string name, SqlConnection connection)
        {
            this.name = name;

            var sqlCommand = new SqlCommand(name, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlCommandBuilder.DeriveParameters(sqlCommand);

            parameters = new SqlParameter[sqlCommand.Parameters.Count];
            sqlCommand.Parameters.CopyTo(parameters, 0);
        }

        /// <summary>
        /// Gets a sql command with populated parameters.
        /// </summary>
        public SqlCommand GetSqlCommand(SqlConnection connection)
        {
            var cmd = new SqlCommand(name, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddRange(parameters);

            return cmd;
        }
    }
}
