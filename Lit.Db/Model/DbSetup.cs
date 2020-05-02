using System;
using System.Collections.Generic;

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
    }
}
