namespace Lit.Db.MySql
{
    /// <summary>
    /// Default naming convention for MySql.
    /// </summary>
    public class MySqlDefaultNaming : DbNaming
    {
        public MySqlDefaultNaming() : base(Placing.Sufix, Case.Snake, "id") { }
    }
}
