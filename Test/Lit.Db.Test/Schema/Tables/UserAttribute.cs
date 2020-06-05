namespace Lit.Db.Test.Schema.Tables
{
    /// <summary>
    /// User attribute value.
    /// </summary>
    [DbTable]
    [DbTablePrimaryKey(nameof(User), nameof(IdAttribute))]
    public class UserAttribute : DbRecordBase
    {
        [DbColumn]
        public IDbForeignKeyRef<User> User { get; set; }

        [DbColumn]
        public IDbForeignKeyRef<User> User2 { get; set; }

        [DbColumn]
        public int IdAttribute { get; set; }

        [DbColumn]
        public string Value { get; set; }
    }
}
