namespace Lit.Db.Attributes
{
    public interface IDbTableKeyAttribute
    {
        /// <summary>
        /// DB name.
        /// </summary>
        string DbName { get; set; }

        /// <summary>
        /// List of field names.
        /// </summary>
        string[] FieldNames { get; }
    }
}
