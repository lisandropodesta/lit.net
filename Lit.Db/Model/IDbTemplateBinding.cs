using System;

namespace Lit.Db
{
    /// <summary>
    /// Template information.
    /// </summary>
    public interface IDbTemplateBinding
    {
        /// <summary>
        /// Template data type.
        /// </summary>
        Type TemplateType { get; }
    }
}
