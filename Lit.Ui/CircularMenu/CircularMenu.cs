using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu.
    /// </summary>
    public abstract class CircularMenu : CircularMenuObjectModel
    {
        /// <summary>
        /// Makes the menu close upon selection.
        /// </summary>
        public bool CloseOnSelection { get; set; }

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
        /// Default background color for items.
        /// </summary>
        public Color ItemsDefaultBackground { get; set; }

        /// <summary>
        /// Default border color for items.
        /// </summary>
        public Color ItemsDefaultBorder { get; set; }

        /// <summary>
        /// Items list.
        /// </summary>
        public IEnumerable<CircularMenuItem> Items { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CircularMenu()
        {
            DisplayFromAngle = Math.PI;
            DisplayToAngle = 0;
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            Show = false;
            Release(rings);
            base.Release();
        }

        #region Render

        /// <summary>
        /// Rings.
        /// </summary>
        internal IReadOnlyList<CircularMenuRing> Rings => rings;

        private readonly List<CircularMenuRing> rings = new List<CircularMenuRing>();

        private bool layoutReady;

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            if (IsShowing && (change >= Change.Layout || !layoutReady))
            {
                Arrange();
                layoutReady = true;
            }

            if (change >= Change.Visibility)
            {
                Rings.ForEach(r => r.Show = Show);
            }

            base.OnPropertyChanged(change, name);
        }

        /// <summary>
        /// Initializes the menu.
        /// </summary>
        private void Arrange()
        {
            rings.ForEach(r => r.ClearItems());

            Items.ForEach(item =>
            {
                item.Menu = this;
                GetRing(item.Ring ?? 0).Add(item);
            });

            rings.ForEach(r => r.ArrangeItems());
        }

        /// <summary>
        /// Get a specific ring.
        /// </summary>
        private CircularMenuRing GetRing(int ringIndex)
        {
            while (ringIndex >= rings.Count)
            {
                var ring = CreateRing();

                var internalRadius = rings.Count == 0 ? CenterRadius : rings[rings.Count - 1].ToRadius;
                ring.FromRadius = internalRadius;
                ring.ToRadius = internalRadius + RingSize;

                rings.Add(ring);
            }

            return rings[ringIndex];
        }

        /// <summary>
        /// Creates a ring.
        /// </summary>
        protected abstract CircularMenuRing CreateRing();

        #endregion
    }
}
