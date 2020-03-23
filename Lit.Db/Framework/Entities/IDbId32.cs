namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// 32 bits integer id.
    /// </summary>
    public interface IDbId32 : IDbId
    {
        int Id { get; set; }
    }
}
