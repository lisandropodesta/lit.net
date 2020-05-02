using Lit.Db.Custom.MySql;

namespace Lit.Db.Test.Schema.Tables
{
    [DbTable]
    [MySqlTable("latin1")]
    [DbTablePrimaryKey(nameof(IdFolder), nameof(Release))]
    [DbTableForeignKey(typeof(Folder), nameof(IdParentFolder), nameof(ParentRelease))]
    [DbTableUniqueKey(nameof(IdUser), nameof(IdParentFolder), nameof(Name))]
    [DbTableIndex(nameof(Name))]
    public class Folder : DbRecordBase
    {
        [DbColumn]
        public int IdFolder { get; set; }

        [DbColumn]
        public int Release { get; set; }

        [DbForeignKey(typeof(User))]
        public int IdUser { get; set; }

        [DbColumn]
        public int? IdParentFolder { get; set; }

        [DbColumn]
        public int? ParentRelease { get; set; }

        [DbColumn]
        public string Name { get; set; }
    }
}
