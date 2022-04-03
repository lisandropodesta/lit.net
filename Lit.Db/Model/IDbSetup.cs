namespace Lit.Db
{
    /// <summary>
    /// Configuration setup for a DB.
    /// </summary>
    public interface IDbSetup : IDbBindingCache
    {
        /// <summary>
        /// Naming manager.
        /// </summary>
        IDbNaming Naming { get; }

        /// <summary>
        /// Translation.
        /// </summary>
        IDbTranslation Translation { get; }
    }
}
