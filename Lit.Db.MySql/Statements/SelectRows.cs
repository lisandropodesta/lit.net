﻿namespace Lit.Db.MySql.Statements
{
    public enum SelectRows
    {
        [DbEnumCode("")]
        All,

        [DbEnumCode("DISTINCT")]
        Distinct
    }
}
