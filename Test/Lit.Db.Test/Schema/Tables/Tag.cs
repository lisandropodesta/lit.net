using Lit.Db.Attributes;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [DbTablePrimaryKey(nameof(Id), nameof(IdTagCategory))]
    [DbTableForeignKey(typeof(Tag), nameof(IdParentTag), nameof(IdTagCategory))]
    [DbTableUniqueKey(nameof(IdParentTag), nameof(Name))]
    public class Tag
    {
        [DbColumn(AutoIncrement = true)]
        public long Id { get; set; }

        [DbForeignKey(typeof(TagCategory))]
        public short IdTagCategory { get; set; }

        [DbColumn]
        public long? IdParentTag { get; set; }

        [DbColumn]
        public string Name { get; set; }
    }
}
