namespace Lit.Db
{
    public interface IDbForeignKeyRef : IDbDataAccessRef
    {
        object KeyAsObject { get; set; }
    }

    public interface IDbForeignKeyRef<TR> : IDbForeignKeyRef
    {
        TR Record { get; }
    }
}
