using System;
using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu configuration.
    /// </summary>
    public class CircularMenu<T> where T : CircularMenuItem
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
        /// Internal radius for the first ring.
        /// </summary>
        public double CenterRadius { get; set; }

        /// <summary>
        /// Ring radial size.
        /// </summary>
        public double RingSize { get; set; }

        /// <summary>
        /// Default items shape.
        /// </summary>
        public CircularItemShape ItemsDefaultShape { get; set; }

        /// <summary>
        /// Items list.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CircularMenu()
        {
            DisplayFromAngle = Math.PI;
            DisplayToAngle = 0;
        }
    }
}
