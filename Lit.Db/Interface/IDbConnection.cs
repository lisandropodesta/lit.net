namespace Lit.Db.Interface
{
    /// <summary>
    /// Data base connection definition.
    /// </summary>
    public interface IDbConnection : IDbCommands
    {
        /// <summary>
        /// Connection string name.
        /// </summary>
        string ConnectionString { get; }
    }
}
