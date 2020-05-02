namespace Lit.Db
{
    /// <summary>
    /// Db parameter binding interface.
    /// </summary>
    public interface IDbParameterBinding : IDbPropertyBinding<DbParameterAttribute>
    {
        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }
    }
}
