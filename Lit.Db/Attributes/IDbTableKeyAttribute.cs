namespace Lit.Db
{
    public interface IDbTableKeyAttribute
    {
        /// <summary>
        /// DB name.
        /// </summary>
        string DbName { get; }

        /// <summary>
        /// List of field names.
        /// </summary>
        string[] PropertyNames { get; }
    }
}
