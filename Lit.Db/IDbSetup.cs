using System;
using Lit.Db.Model;

namespace Lit.Db
{
    /// <summary>
    /// Configuration setup for a DB.
    /// </summary>
    public interface IDbSetup
    {
        /// <summary>
        /// Naming manager.
        /// </summary>
        IDbNaming Naming { get; }

        /// <summary>
        /// Translation.
        /// </summary>
        IDbTranslation Translation { get; }

        /// <summary>
        /// Gets a table template.
        /// </summary>
        DbTemplateBinding GetTableBinding(Type type);

        /// <summary>
        /// Gets the template binding information.
        /// </summary>
        DbTemplateBinding GetTemplateBinding(Type type);
    }
}
