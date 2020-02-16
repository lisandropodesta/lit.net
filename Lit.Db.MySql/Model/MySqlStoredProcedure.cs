using System.Data;
using MySql.Data.MySqlClient;
using Lit.Db.Model;

namespace Lit.Db.MySql.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class MySqlStoredProcedure : DbStoredProcedure<MySqlConnection, MySqlCommand>
    {
        private readonly MySqlParameter[] parameters;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MySqlStoredProcedure(string name, MySqlConnection connection)
            : base(name)
        {
            var cmd = CreateCommand(name, connection);

            MySqlCommandBuilder.DeriveParameters(cmd);

            parameters = new MySqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(parameters, 0);
        }

        /// <summary>
        /// Gets a sql command with populated parameters.
        /// </summary>
        protected override MySqlCommand CreateCommand(string name, MySqlConnection connection)
        {
            var cmd = new MySqlCommand(name, connection)
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
