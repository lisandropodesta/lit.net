using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lit.Db.Model
{
    /// <summary>
    /// Template information.
    /// </summary>
    internal abstract class DbTemplateBinding
    {
        /// <summary>
        /// Template data type.
        /// </summary>
        public Type TemplateType => templateType;

        private readonly Type templateType;

        protected readonly IDbSetup Setup;

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
        protected TI AddBinding<TI, TAttr>(ref List<TI> list, Type genClassType, PropertyInfo propertyInfo, TAttr attribute, IDbSetup setup)
            where TI : class
        {
            if (list == null)
            {
                list = new List<TI>();
            }

            var binding = CreateBinding<TI, TAttr>(genClassType, propertyInfo, attribute, setup);
            list.Add(binding);
            return binding;
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        private TI CreateBinding<TI, TAttr>(Type genClassType, PropertyInfo propertyInfo, TAttr attribute, IDbSetup setup)
            where TI : class
        {
            var type = genClassType.MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var binding = Activator.CreateInstance(type, new object[] { setup, propertyInfo, attribute });
            return binding as TI;
        }
    }
}
