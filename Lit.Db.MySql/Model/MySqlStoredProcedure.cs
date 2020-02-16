using MySql.Data.MySqlClient;
using Lit.Db.Model;

namespace Lit.Db.MySql.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class MySqlStoredProcedure : DbStoredProcedureBase<MySqlConnection, MySqlCommand, MySqlParameter>
    {
        #region Constructors

        public MySqlStoredProcedure(string name, MySqlConnection connection) : base(name) { }

        #endregion

        protected override MySqlCommand CreateCommandRaw(string name, MySqlConnection connection)
        {
            return new MySqlCommand(name, connection);
        }

        protected override void DeriveParameters(MySqlCommand command)
        {
            MySqlCommandBuilder.DeriveParameters(command);
        }
    }
}
