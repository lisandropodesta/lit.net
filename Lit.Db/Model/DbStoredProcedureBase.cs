using System.Data;
using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    public abstract class DbStoredProcedureBase<TS, TP> : DbStoredProcedure<TS>
        where TS : DbCommand
        where TP : DbParameter
    {
        private readonly TP[] parameters;

        #region Constructors

        protected DbStoredProcedureBase(string name, TS command)
            : base(name)
        {
            command.CommandType = CommandType.StoredProcedure;

            DeriveParameters(command);

            parameters = new TP[command.Parameters.Count];
            command.Parameters.CopyTo(parameters, 0);
        }

        #endregion

        public override void AddParameters(TS command)
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
        }

        /// <summary>
        /// Get parameters from db.
        /// </summary>
        protected abstract void DeriveParameters(TS command);
    }
}
