using System.Data.Common;
using Lit.Db.Attributes;

namespace Lit.Db.Model
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
