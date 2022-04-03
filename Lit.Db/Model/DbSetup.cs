using System;
using System.Collections.Generic;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// DB configuration setup.
    /// </summary>
    public class DbSetup : IDbSetup
    {
        // Table binding cache
        private readonly Dictionary<Type, IDbTableBinding> tableBindings = new Dictionary<Type, IDbTableBinding>();

        // Stored procedure binding cache
        private readonly Dictionary<Type, IDbCommandBinding> commandBindings = new Dictionary<Type, IDbCommandBinding>();

        // Parameters binding cache
        private readonly Dictionary<Type, IReadOnlyList<IDbParameterBinding>> parametersBinding = new Dictionary<Type, IReadOnlyList<IDbParameterBinding>>();

        #region Constructor

        public DbSetup(IDbNaming naming, IDbTranslation translation)
        {
            Naming = naming;
            Translation = translation;
        }

        #endregion

        /// <summary>
        /// Naming manager.
        /// </summary>
        public IDbNaming Naming { get; private set; }

        /// <summary>
        /// Encoding.
        /// </summary>
        public IDbTranslation Translation { get; private set; }

        /// <summary>
        /// Gets a table binding.
        /// </summary>
        public IDbTableBinding GetTableBinding(Type type)
        {
            lock (tableBindings)
            {
                if (!tableBindings.TryGetValue(type, out var binding))
                {
                    var newBinding = new DbTableBinding(type, this);
                    binding = newBinding;
                    tableBindings.Add(type, binding);

                    newBinding.ResolveBinding();
                }

                return binding;
            }
        }

        /// <summary>
        /// Gets a command binding.
        /// </summary>
        public IDbCommandBinding GetCommandBinding(Type type)
        {
            lock (commandBindings)
            {
                if (!commandBindings.TryGetValue(type, out var binding))
                {
                    var newBinding = new DbCommandBinding(type, this);
                    binding = newBinding;
                    commandBindings.Add(type, binding);

                    newBinding.ResolveBinding();
                }

                return binding;
            }
        }

        /// <summary>
        /// Gets parameters binding.
        /// </summary>
        public IReadOnlyList<IDbParameterBinding> GetParametersBinding(Type type)
        {
            lock (parametersBinding)
            {
                if (!parametersBinding.TryGetValue(type, out var bindingList))
                {
                    var newBindingList = new List<IDbParameterBinding>();

                    foreach (var propInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var typeArguments = new[] { type, propInfo.PropertyType };

                        if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                        {
                            var binding = TypeHelper.AddBinding(newBindingList, typeof(DbParameterBinding<,>), typeArguments, this, propInfo, pAttr);
                            binding.CalcBindingMode();
                        }
                    }

                    bindingList = newBindingList;
                    parametersBinding.Add(type, bindingList);
                }

                return bindingList;
            }
        }
    }
}
