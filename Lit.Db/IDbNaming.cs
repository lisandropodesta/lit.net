using System;
using System.Reflection;

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
        string GetParameterName(PropertyInfo propInfo, string parameterName);

        /// <summary>
        /// Gets a field name.
        /// </summary>
        string GetFieldName(PropertyInfo propInfo, string fieldName);

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        string GetStoredProcedureName(Type template, string spName);

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        string GetStoredProcedureName(string spName);

        /// <summary>
        /// Gets a table name.
        /// </summary>
        string GetTableName(Type tableType, string tableName);

        /// <summary>
        /// Gets a table column name.
        /// </summary>
        string GetColumnName(string tableName, PropertyInfo propInfo, string fieldName);
    }
}
