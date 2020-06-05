using System;
using System.Reflection;
using Lit.Db.Framework;

namespace Lit.Db
{
    /// <summary>
    /// Db naming management.
    /// </summary>
    public interface IDbNaming
    {
        /// <summary>
        /// Gets a parameter name.
        /// </summary>
        string GetParameterName(PropertyInfo propInfo, string columnName, string parameterName, bool doNotTranslate = false, DbKeyConstraint constraint = DbKeyConstraint.None);

        /// <summary>
        /// Gets a field name.
        /// </summary>
        string GetFieldName(PropertyInfo propInfo, string fieldName, DbKeyConstraint constraint = DbKeyConstraint.None);

        /// <summary>
        /// Gets a table column name.
        /// </summary>
        string GetColumnName(string tableName, PropertyInfo propInfo, string columnName, DbKeyConstraint constraint = DbKeyConstraint.None);

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        string GetStoredProcedureName(Type spTemplate, string spName);

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        string GetStoredProcedureName(string tableName, StoredProcedureFunction spFunc);

        /// <summary>
        /// Gets a table name.
        /// </summary>
        string GetTableName(Type tableType, string tableName);

        /// <summary>
        /// Gets a table name as it needs to be put inside a SQL sentence.
        /// </summary>
        string GetSqlTableName(string name);

        /// <summary>
        /// Gets a stored procedure name as it needs to be put inside a SQL sentence.
        /// </summary>
        string GetSqlSpName(string name);

        /// <summary>
        /// Gets the column name as it needs to be put inside a SQL sentence.
        /// </summary>
        string GetSqlColumnName(string name);

        /// <summary>
        /// Gets the parameter name related to a column as it needs to be put inside a SQL sentence.
        /// </summary>
        string GetSqlParamName(string name);

        /// <summary>
        /// Gets a SQL type name.
        /// </summary>
        string GetSqlType(DbDataType dataType, Type type = null, ulong? size = null, int? precision = null);
    }
}
