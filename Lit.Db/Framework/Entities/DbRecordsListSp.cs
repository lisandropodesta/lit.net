using System.Collections.Generic;
using Lit.Db.Attributes;

namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// Helper class to hold a list of records.
    /// </summary>
    [DbStoredProcedure]
    public class DbRecordsListSp<T>
    {
        [DbRecordset]
        public List<T> Result { get; protected set; }
    }
}
