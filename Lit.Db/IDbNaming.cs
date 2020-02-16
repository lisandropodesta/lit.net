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
    }
}
