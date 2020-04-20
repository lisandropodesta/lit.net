using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    public abstract class DbStoredProcedure<TS>
        where TS : DbCommand
    {
        private readonly string name;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DbStoredProcedure(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Add the parameters to the command.
        /// </summary>
        public abstract void AddParameters(TS command);
    }
}
