namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// Integer id.
    /// </summary>
    public interface IDbId<T>
    {
        T Id { get; set; }
    }
}
