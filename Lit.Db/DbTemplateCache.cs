using System;
using System.Collections.Generic;
using System.Data;
using Lit.Db.Model;

namespace Lit.Db
{
    /// <summary>
    /// Template cache.
    /// </summary>
    public static class DbTemplateCache
    {
        // Templates cache
        private static readonly Dictionary<Type, DbTemplateBinding> templateBindings = new Dictionary<Type, DbTemplateBinding>();

        /// <summary>
        /// Gets a table template.
        /// </summary>
        public static DbTemplateBinding GetTable(Type type, IDbNaming dbNaming)
        {
            var binding = Get(type, dbNaming);
            if (binding.CommandType != CommandType.TableDirect)
            {
                throw new ArgumentException($"Invalid table template on type [{type.FullName}]");
            }

            return binding;
        }

        /// <summary>
        /// Gets the template binding information.
        /// </summary>
        public static DbTemplateBinding Get(Type type, IDbNaming dbNaming)
        {
            lock (templateBindings)
            {
                if (!templateBindings.TryGetValue(type, out var template))
                {
                    template = new DbTemplateBinding(type, dbNaming);
                    templateBindings.Add(type, template);
                    template.ResolveForeignKeys(dbNaming);
                }

                return template;
            }
        }
    }
}
