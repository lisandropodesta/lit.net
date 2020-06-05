using System;
using Lit.Names;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Default naming convention for Sql.
    /// </summary>
    public class SqlDefaultNaming : DbNaming
    {
        public SqlDefaultNaming() : base(AffixPlacing.Prefix, Case.Pascal, "Id")
        {
            ForceIdOnKeyColumn = true;
        }

        public override string GetSqlType(DbDataType dataType, Type type = null, ulong? size = null, int? precision = null)
        {
            throw new NotImplementedException();
        }
    }
}
