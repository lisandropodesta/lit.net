namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// 64 bits integer id.
    /// </summary>
    public interface IDbId64 : IDbId
    {
        long Id { get; set; }
    }
}
