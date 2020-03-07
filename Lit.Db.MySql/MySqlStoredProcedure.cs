using MySql.Data.MySqlClient;
using Lit.Db.Model;

namespace Lit.Db.MySql
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class MySqlStoredProcedure : DbStoredProcedureBase<MySqlCommand, MySqlParameter>
    {
        #region Constructors

        public MySqlStoredProcedure(string name, MySqlCommand command) : base(name, command) { }

        #endregion

        protected override void DeriveParameters(MySqlCommand command)
        {
            MySqlCommandBuilder.DeriveParameters(command);
        }
    }
}
