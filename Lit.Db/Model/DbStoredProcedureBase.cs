using System.Data;
using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Stored procedure information.
    /// </summary>
    public abstract class DbStoredProcedureBase<TH, TS, TP> : DbStoredProcedure<TH, TS>
        where TH : DbConnection
        where TS : DbCommand
        where TP : DbParameter
    {
        private bool parametersLoaded;

        private TP[] parameters;

        #region Constructors

        protected DbStoredProcedureBase(string name) : base(name) { }

        #endregion

        /// <summary>
        /// Gets a sql command with populated parameters.
        /// </summary>
        protected override TS CreateCommand(string name, TH connection)
        {
            var cmd = CreateCommandRaw(name, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (!parametersLoaded)
            {
                DeriveParameters(cmd);

                parameters = new TP[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(parameters, 0);

                parametersLoaded = true;
            }
            else if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }

        /// <summary>
        /// Creates a raw command without parameters.
        /// </summary>
        protected abstract TS CreateCommandRaw(string name, TH connection);

        /// <summary>
        /// Get parameters from db.
        /// </summary>
        protected abstract void DeriveParameters(TS command);
    }
}
