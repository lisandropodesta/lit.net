using System.Data.Common;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db parameter binding interface.
    /// </summary>
    public interface IDbParameterBinding : IDbPropertyBinding<DbParameterAttribute>
    {
        /// <summary>
        /// Name of the standard stored procedure parameter.
        /// </summary>
        string SpParamName { get; }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(ref string text, object instance);

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);

        /// <summary>
        /// Get output parameters.
        /// </summary>
        void GetOutputParameters(DbCommand cmd, object instance);
    }
}
