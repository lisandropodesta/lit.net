using System.Data.Common;
using Lit.Db.Attributes;

namespace Lit.Db.Model
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
