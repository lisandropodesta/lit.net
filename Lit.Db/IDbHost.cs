namespace Lit.Db
{
    /// <summary>
    /// Data base host definition.
    /// </summary>
    public interface IDbHost : IDbCommands
    {
        /// <summary>
        /// Connection string name.
        /// </summary>
        string ConnectionString { get; }
    }
}
