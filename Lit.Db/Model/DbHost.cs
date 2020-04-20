using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Database host.
    /// </summary>
    public abstract class DbHost<TH, TS> : DbCommands<TH, TS>, IDbHost
        where TH : DbConnection
        where TS : DbCommand
    {
        /// <summary>
        /// Connection string name.
        /// </summary>
        public string ConnectionString => connectionString;

        private readonly string connectionString;

        #region Constructors

        protected DbHost(IDbSetup setup, string connectionString) : base(setup)
        {
            this.connectionString = connectionString;
        }

        #endregion
    }
}
