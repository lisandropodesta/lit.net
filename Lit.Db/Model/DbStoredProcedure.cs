using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    public abstract class DbStoredProcedure<TH, TS>
        where TH : DbConnection
        where TS : DbCommand
    {
        private readonly string name;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DbStoredProcedure(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets a command with populated parameters.
        /// </summary>
        public TS GetCommand(TH connection)
        {
            return CreateCommand(name, connection);
        }

        protected abstract TS CreateCommand(string name, TH connection);
    }
}
