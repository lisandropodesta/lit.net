using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Db recordset binding interface.
    /// </summary>
    public interface IDbRecordsetBinding : IDbPropertyBinding<DbRecordsetAttribute>
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance);
    }
}
