namespace Lit.Db.Model
{
    public enum DbKeyConstraint
    {
        None,

        PrimaryKey,

        PrimaryForeignKey,

        UniqueKey,

        ForeignKey
    }
}
