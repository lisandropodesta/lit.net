namespace Lit.Db.PostgreSQL
{
    /// <summary>
    /// PostgreSQL template.
    /// </summary>
    public class PostgresTemplate : DbTemplate
    {
        #region String constants

        public const string AllColumnsKey = "*";

        public const string NullKey = "NULL";

        public const string NotNullKey = "NOT NULL";

        public const string SerialKey = "SERIAL";

        public const string BigSerialKey = "BIGSERIAL";

        public const string KeyKey = "KEY";

        public const string IndexKey = "INDEX";

        public const string ConstraintKey = "CONSTRAINT";

        public const string PrimaryKeyKey = "PRIMARY KEY";

        public const string ForeignKeyKey = "FOREIGN KEY";

        public const string UniqueKeyKey = "UNIQUE";

        public const string ReferencesKey = "REFERENCES";

        public const string DefaultKey = "DEFAULT";

        public const string IfExistsKey = "IF EXISTS";

        public const string IfNotExistsKey = "IF NOT EXISTS";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public PostgresTemplate(IDbSetup setup) : base(setup)
        {
        }

        protected string GetNullable(IDbColumnBinding column)
        {
            return column.IsNullable ? NullKey : NotNullKey;
        }

        protected string GetDefault(IDbColumnBinding column)
        {
            // TODO: implement this code!
            return string.Empty;
        }

        protected string GetAutoIncrement(IDbColumnBinding column)
        {
            // In PostgreSQL, IDENTITY GENERATED ALWAYS AS IDENTITY is the standard way
            return column.IsAutoIncrement ? "GENERATED ALWAYS AS IDENTITY" : string.Empty;
        }
    }
}
