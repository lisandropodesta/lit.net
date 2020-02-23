﻿using Lit.Names;

namespace Lit.Db.Sql
{
    /// <summary>
    /// Default naming convention for Sql.
    /// </summary>
    public class SqlDefaultNaming : DbNaming
    {
        public SqlDefaultNaming() : base(AffixPlacing.Prefix, Case.Pascal, "Id") { }
    }
}
