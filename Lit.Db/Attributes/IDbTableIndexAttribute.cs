namespace Lit.Db
{
    public interface IDbTableIndexAttribute : IDbTableKeyAttribute
    {
        /// <summary>
        /// Is unique flag.
        /// </summary>
        bool IsUnique { get; }
    }
}
