using Lit.Names;

namespace Lit.Db.MySql
{
    /// <summary>
    /// Default naming convention for MySql.
    /// </summary>
    public class MySqlDefaultNaming : DbNaming
    {
        public MySqlDefaultNaming() : base(AffixPlacing.Sufix, Case.Snake, "id") { }

        public override string GetParameterName(string reflectionName, string columnName, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                parameterName = "p_" + (columnName ?? reflectionName);
            }

            return base.GetParameterName(null, null, parameterName);
        }
    }
}
