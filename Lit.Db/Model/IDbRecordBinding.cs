using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Db record binding interface.
    /// </summary>
    public interface IDbRecordBinding : IDbPropertyBinding<DbRecordAttribute>
    {
        /// <summary>
        /// Load the current recordset.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance);
    }
}
