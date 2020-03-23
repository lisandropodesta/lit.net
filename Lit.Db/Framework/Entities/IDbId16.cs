namespace Lit.Db.Framework.Entities
{
    /// <summary>
    /// 16 bits integer id.
    /// </summary>
    public interface IDbId16 : IDbId
    {
        short Id { get; set; }
    }
}
