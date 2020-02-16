namespace Lit.Db.Sql
{
    /// <summary>
    /// Default naming convention for Sql.
    /// </summary>
    public class SqlDefaultNaming : DbNaming
    {
        public SqlDefaultNaming() : base(Placing.Prefix, Case.Pascal, "Id") { }
    }
}
