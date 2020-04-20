using System.Data.SqlClient;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class SqlStoredProcedure : DbStoredProcedureBase<SqlCommand, SqlParameter>
    {
        #region Constructors

        public SqlStoredProcedure(string name, SqlCommand command) : base(name, command) { }

        #endregion

        protected override void DeriveParameters(SqlCommand command)
        {
            SqlCommandBuilder.DeriveParameters(command);
        }
    }
}
