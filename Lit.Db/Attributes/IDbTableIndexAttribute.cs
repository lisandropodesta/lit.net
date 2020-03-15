namespace Lit.Db.Attributes
{
    public interface IDbTableIndexAttribute : IDbTableKeyAttribute
    {
        /// <summary>
        /// Is unique flag.
        /// </summary>
        bool IsUnique { get; }
    }
}
