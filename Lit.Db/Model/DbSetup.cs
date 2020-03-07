using System;
using System.Collections.Generic;
using System.Data;

namespace Lit.Db.Model
{
    /// <summary>
    /// DB configuration setup.
    /// </summary>
    public class DbSetup : IDbSetup
    {
        // Templates cache
        private readonly Dictionary<Type, DbTemplateBinding> templateBindings = new Dictionary<Type, DbTemplateBinding>();

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
        /// Gets a table template.
        /// </summary>
        public DbTemplateBinding GetTableBinding(Type type)
        {
            var binding = GetTemplateBinding(type);
            if (binding.CommandType != CommandType.TableDirect)
            {
                throw new ArgumentException($"Invalid table template on type [{type.FullName}]");
            }

            return binding;
        }

        /// <summary>
        /// Gets the template binding information.
        /// </summary>
        public DbTemplateBinding GetTemplateBinding(Type type)
        {
            lock (templateBindings)
            {
                if (!templateBindings.TryGetValue(type, out var template))
                {
                    template = new DbTemplateBinding(type, this);
                    templateBindings.Add(type, template);
                    template.ResolveForeignKeys();
                }

                return template;
            }
        }
    }
}
