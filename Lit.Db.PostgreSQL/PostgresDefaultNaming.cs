using System;
using System.Reflection;
using Lit.Names;

namespace Lit.Db.PostgreSQL
{
    public class PostgresDefaultNaming : DbNaming
    {
        public PostgresDefaultNaming() : base(AffixPlacing.Sufix, Case.Snake, "id")
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

        /// <summary>
        /// Gets a SQL type name.
        /// </summary>
        public override string GetSqlType(DbDataType dataType, Type type = null, ulong? size = null, int? precision = null)
        {
            return PostgresDataType.Translate(dataType, type, size, precision);
        }
    }
}
