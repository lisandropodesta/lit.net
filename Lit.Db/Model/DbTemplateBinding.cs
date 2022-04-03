using System;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Template information.
    /// </summary>
    internal abstract class DbTemplateBinding : IDbTemplateBinding
    {
        /// <summary>
        /// Database setup.
        /// </summary>
        public IDbSetup Setup { get; private set; }

        /// <summary>
        /// Template data type.
        /// </summary>
        public Type TemplateType { get; private set; }

        #region Constructor

        protected DbTemplateBinding(Type templateType, IDbSetup setup)
        {
            TemplateType = templateType;
            Setup = setup;
        }

        #endregion
    }
}
