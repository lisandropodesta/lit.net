using System.Data;
using System.Data.SqlClient;
using Lit.Db.Model;

namespace Lit.Db.Sql.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class SqlStoredProcedure : DbStoredProcedure<SqlConnection, SqlCommand>
    {
        private readonly SqlParameter[] parameters;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlStoredProcedure(string name, SqlConnection connection)
            : base(name)
        {
            var cmd = CreateCommand(name, connection);

            SqlCommandBuilder.DeriveParameters(cmd);

            parameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(parameters, 0);
        }

        /// <summary>
        /// Gets a sql command with populated parameters.
        /// </summary>
        protected override SqlCommand CreateCommand(string name, SqlConnection connection)
        {
            var cmd = new SqlCommand(name, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }
    }
}
