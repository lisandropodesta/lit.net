using System.Reflection;
using Lit.Names;

namespace Lit.Db.MySql
{
    /// <summary>
    /// Default naming convention for MySql.
    /// </summary>
    public class MySqlDefaultNaming : DbNaming
    {
        public MySqlDefaultNaming() : base(AffixPlacing.Sufix, Case.Snake, "id")
        {
            ForceIdOnKeyColumn = true;
        }

        public override string GetParameterName(PropertyInfo propInfo, string columnName, string parameterName, DbKeyConstraint constraint = DbKeyConstraint.None)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                parameterName = "p_" + (columnName ?? propInfo.Name);
            }

            return base.GetParameterName(null, null, parameterName);
        }
    }
}
