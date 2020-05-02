namespace Lit.Db
{
    public interface IDbDataAccessRef
    {
        IDbDataAccess Db { get; set; }
    }
}
