using Npgsql;

namespace Lit.Db.PostgreSQL
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    internal class PostgresStoredProcedure : DbStoredProcedureBase<NpgsqlCommand, NpgsqlParameter>
    {
        #region Constructors

        public PostgresStoredProcedure(string name, NpgsqlCommand command) : base(name, command) { }

        #endregion

        protected override void DeriveParameters(NpgsqlCommand command)
        {
            NpgsqlCommandBuilder.DeriveParameters(command);
        }
    }
}
