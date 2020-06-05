namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql template.
    /// </summary>
    public class MySqlTemplate : DbTemplate
    {
        #region String constants

        public const string AllColumnsKey = "*";

        public const string NullKey = "NULL";

        public const string NotNullKey = "NOT NULL";

        public const string AutoIncrementKey = "AUTO_INCREMENT";

        public const string KeyKey = "KEY";

        public const string IndexKey = "INDEX";

        public const string ConstraintKey = "CONSTRAINT";

        public const string PrimaryKeyKey = "PRIMARY KEY";

        public const string ForeignKeyKey = "FOREIGN KEY";

        public const string UniqueKeyKey = "UNIQUE KEY";

        public const string ReferecencesKey = "REFERENCES";

        public const string DefaultKey = "DEFAULT";

        public const string IfExistsKey = "IF EXISTS";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="setup"></param>
        public MySqlTemplate(IDbSetup setup) : base(setup)
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
            return column.IsAutoIncrement ? AutoIncrementKey : string.Empty;
        }
    }
}
