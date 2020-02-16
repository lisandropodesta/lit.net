using System.Data.SqlClient;
using Lit.Db.Model;

namespace Lit.Db.Sql.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class SqlStoredProcedure : DbStoredProcedureBase<SqlConnection, SqlCommand, SqlParameter>
    {
        #region Constructors

        public SqlStoredProcedure(string name, SqlConnection connection) : base(name) { }

        #endregion

        protected override SqlCommand CreateCommandRaw(string name, SqlConnection connection)
        {
            return new SqlCommand(name, connection);
        }

        protected override void DeriveParameters(SqlCommand command)
        {
            SqlCommandBuilder.DeriveParameters(command);
        }
    }
}
