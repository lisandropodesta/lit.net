using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu configuration.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Sort items by catergory.
        /// </summary>
        public bool SortByCategory { get; set; }

        /// <summary>
        /// Display from angle.
        /// </summary>
        public double DisplayFromAngle { get; set; }

        /// <summary>
        /// Display to angle.
        /// </summary>
        public double DisplayToAngle { get; set; }

        /// <summary>
        /// Margin between categories.
        /// </summary>
        public double CategoryMargin { get; set; }

        /// <summary>
        /// Margin between items inside same category.
        /// </summary>
        public double ItemsMargin { get; set; }

        /// <summary>
        /// Minimum radial size for each item.
        /// </summary>
        public double MinimumItemSize { get; set; }

        /// <summary>
        /// Default items shape.
        /// </summary>
        public ItemShape ItemsDefaultShape { get; set; }

        /// <summary>
        /// Default background color for items.
        /// </summary>
        public Color ItemsDefaultBackground { get; set; }

        /// <summary>
        /// Default border color for items.
        /// </summary>
        public Color ItemsDefaultBorder { get; set; }

        /// <summary>
        /// Action to be executed.
        /// </summary>
        public Action<MenuItem, object> DefaultCommand { get; set; }

        /// <summary>
        /// Items list.
        /// </summary>
        public IEnumerable<MenuItem> Items { get; set; }
    }
}
