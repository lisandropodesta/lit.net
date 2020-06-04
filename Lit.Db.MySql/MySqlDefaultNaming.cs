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

        public override string GetSqlTableName(string name)
        {
            return "`" + name + "`";
        }

        public override string GetSqlSpName(string name)
        {
            return "`" + name + "`";
        }

        public override string GetSqlColumnName(string name)
        {
            return "`" + name + "`";
        }

        public override string GetParameterName(PropertyInfo propInfo, string columnName, string parameterName, bool doNotTranslate = false, DbKeyConstraint constraint = DbKeyConstraint.None)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                parameterName = (doNotTranslate ? string.Empty : "p_") + (columnName ?? propInfo.Name);
            }

            return base.GetParameterName(null, null, parameterName, doNotTranslate, constraint);
        }
    }
}
