using System;
using System.Linq;

namespace Lit.Db.Model
{
    /// <summary>
    /// Helper functions for a template binding.
    /// </summary>
    public partial class DbTemplateBinding
    {
        /// <summary>
        /// Get the first column that matches with selection.
        /// </summary>
        public IDbColumnBinding FindFirstColumn(DbColumnsSelection selection)
        {
            return Columns.FirstOrDefault(c => IsSelected(c, selection));
        }

        /// <summary>
        /// Maps an action to every column.
        /// </summary>
        public void MapColumns(DbColumnsSelection selection, Action<IDbColumnBinding> action)
        {
            Columns.Where(c => IsSelected(c, selection)).ForEach(c => action(c));
        }

        /// <summary>
        /// Check whether if a column matches a selection criteria or not.
        /// </summary>
        public static bool IsSelected(IDbColumnBinding column, DbColumnsSelection selection)
        {
            switch (selection)
            {
                case DbColumnsSelection.None:
                    return false;

                case DbColumnsSelection.PrimaryKey:
                    return column.KeyConstraint == DbKeyConstraint.PrimaryKey;

                case DbColumnsSelection.UniqueKey:
                    return column.KeyConstraint == DbKeyConstraint.UniqueKey;

                case DbColumnsSelection.NonPrimaryKey:
                    return column.KeyConstraint != DbKeyConstraint.PrimaryKey;

                case DbColumnsSelection.All:
                default:
                    return true;
            }
        }
    }
}
