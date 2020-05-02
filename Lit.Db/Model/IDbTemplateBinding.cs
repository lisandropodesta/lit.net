using System;

namespace Lit.Db
{
    /// <summary>
    /// Template information.
    /// </summary>
    public interface IDbTemplateBinding
    {
        /// <summary>
        /// Database setup.
        /// </summary>
        IDbSetup Setup { get; }

        /// <summary>
        /// Template data type.
        /// </summary>
        Type TemplateType { get; }
    }
}
