using System;
using System.Reflection;
using Lit.Db.Architecture;

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
        string GetParameterName(string reflectionName, string parameterName);

        /// <summary>
        /// Gets a field name.
        /// </summary>
        string GetFieldName(string reflectionName, string fieldName);

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
        /// Gets a table column name.
        /// </summary>
        string GetColumnName(string tableName, PropertyInfo propInfo, string fieldName);
    }
}
