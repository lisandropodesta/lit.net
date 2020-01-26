using System;
using System.Collections.Generic;

namespace Lit.Ui
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Applies an action for each item in the enumeration.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
        }
    }
}
