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

        /// <summary>
        /// Adds a binding to a list.
        /// </summary>
        protected TI AddBinding<TI, TA>(ref TypeBinding<TI, TA> bindings, Type genericType, Type[] typeArguments, params object[] instanceArguments) where TI : class where TA : class
        {
            if (bindings == null)
            {
                bindings = new TypeBinding<TI, TA>();
            }

            return bindings.AddBinding(genericType, typeArguments, instanceArguments);
        }
    }
}
