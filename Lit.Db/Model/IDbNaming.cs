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
        string GetParameterName(PropertyInfo propInfo, string columnName, string parameterName, DbKeyConstraint constraint = DbKeyConstraint.None);

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
    }
}
