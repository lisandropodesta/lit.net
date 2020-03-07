using System;
using System.Collections.Generic;

namespace Lit.Db.Model
{
    /// <summary>
    /// DB configuration setup.
    /// </summary>
    public class DbSetup : IDbSetup
    {
        // Table binding cache
        private readonly Dictionary<Type, DbTableBinding> tableBindings = new Dictionary<Type, DbTableBinding>();

        // Stored procedure binding cache
        private readonly Dictionary<Type, DbCommandBinding> commandBindings = new Dictionary<Type, DbCommandBinding>();

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
        public DbTableBinding GetTableBinding(Type type)
        {
            lock (tableBindings)
            {
                if (!tableBindings.TryGetValue(type, out var binding))
                {
                    binding = new DbTableBinding(type, this);
                    tableBindings.Add(type, binding);
                    binding.ResolveForeignKeys();
                }

                return binding;
            }
        }

        /// <summary>
        /// Gets a command binding.
        /// </summary>
        public DbCommandBinding GetCommandBinding(Type type)
        {
            lock (commandBindings)
            {
                if (!commandBindings.TryGetValue(type, out var binding))
                {
                    binding = new DbCommandBinding(type, this);
                    commandBindings.Add(type, binding);
                }

                return binding;
            }
        }
    }
}
