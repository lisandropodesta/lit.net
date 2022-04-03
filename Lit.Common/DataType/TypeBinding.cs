using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// Type binding.
    /// </summary>
    /// <typeparam name="TA">Property attributes class</typeparam>
    public class TypeBinding<TA> : TypeBinding<IAttrPropertyBinding<TA>, TA> where TA : class
    {
        #region Constructors

        public TypeBinding(Type bindedType)
            : base(bindedType, DefaultBindingFlags, DefaultCreateInstance)
        {

        }

        #endregion

        /// <summary>
        /// Default creation of binding instance.
        /// </summary>
        protected static IAttrPropertyBinding<TA> DefaultCreateInstance(PropertyInfo propInfo, TA attr)
        {
            var genericParams = new[] { propInfo.DeclaringType, propInfo.PropertyType, typeof(TA) };
            return TypeHelper.CreateInstance(typeof(AttrPropertyBinding<,,>), genericParams, propInfo, attr, false, false) as IAttrPropertyBinding<TA>;
        }
    }

    /// <summary>
    /// Type binding.
    /// </summary>
    /// <typeparam name="TI">Items interface type</typeparam>
    /// <typeparam name="TA">Property attributes class</typeparam>
    public class TypeBinding<TI, TA> : ITypeBinding<TI> where TI : class where TA : class
    {
        protected const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Binding list.
        /// </summary>
        public IReadOnlyList<TI> BindingList => list;

        private readonly List<TI> list = new List<TI>();

        #region Constructors

        public TypeBinding()
        {
        }

        public TypeBinding(Type bindedType, Func<PropertyInfo, TA, TI> createInstace)
            : this(bindedType, DefaultBindingFlags, createInstace)
        {
        }

        public TypeBinding(Type bindedType, BindingFlags bindingAttr, Func<PropertyInfo, TA, TI> createInstace)
        {
            foreach (var propInfo in bindedType.GetProperties(bindingAttr))
            {
                if (TypeHelper.GetAttribute<TA>(propInfo, out var cAttr))
                {
                    var binding = createInstace(propInfo, cAttr);
                    list.Add(binding);
                }
            }
        }

        #endregion

        /// <summary>
        /// Adds a property binding.
        /// </summary>
        public void AddBinding(TI binding)
        {
            list.Add(binding);
        }
    }
}
