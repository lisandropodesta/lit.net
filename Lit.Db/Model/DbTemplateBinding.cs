using System;
using System.Collections.Generic;
using System.Reflection;

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
        public Type TemplateType => templateType;

        private readonly Type templateType;

        #region Constructor

        protected DbTemplateBinding(Type templateType, IDbSetup setup)
        {
            this.templateType = templateType;
            Setup = setup;
        }

        #endregion

        /// <summary>
        /// Adds a binding to a list.
        /// </summary>
        protected TI AddBinding<TI, TAttr>(ref List<TI> list, Type genClassType, PropertyInfo propertyInfo, TAttr attribute)
            where TI : class
        {
            if (list == null)
            {
                list = new List<TI>();
            }

            var binding = CreateBinding<TI, TAttr>(genClassType, propertyInfo, attribute);
            list.Add(binding);
            return binding;
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        private TI CreateBinding<TI, TAttr>(Type genClassType, PropertyInfo propertyInfo, TAttr attribute)
            where TI : class
        {
            var type = genClassType.MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var binding = Activator.CreateInstance(type, new object[] { this, propertyInfo, attribute });
            return binding as TI;
        }
    }
}
