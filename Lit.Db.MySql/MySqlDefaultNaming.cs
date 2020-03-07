using Lit.Db.Model;
using Lit.Names;

namespace Lit.Db.MySql
{
    /// <summary>
    /// Default naming convention for MySql.
    /// </summary>
    public class MySqlDefaultNaming : DbNaming
    {
        public MySqlDefaultNaming() : base(AffixPlacing.Sufix, Case.Snake, "id") { }

        public override string GetParameterName(string reflectionName, string parameterName)
        {
            reflectionName = !string.IsNullOrEmpty(reflectionName) ? "p_" + reflectionName : null;
            return base.GetParameterName(reflectionName, parameterName);
        }
    }
}
