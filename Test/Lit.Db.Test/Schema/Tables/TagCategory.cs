﻿namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    public class TagCategory : DbRecordBase
    {
        [DbPrimaryKey]
        public short Id { get; set; }

        [DbUniqueKey]
        public string Code { get; set; }
    }
}
