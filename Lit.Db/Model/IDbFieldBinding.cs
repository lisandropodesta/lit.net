namespace Lit.Db
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbFieldBinding : IDbPropertyBinding<DbFieldAttribute>
    {
        /// <summary>
        /// Field name.
        /// </summary>
        string FieldName { get; }
    }
}
