using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu model.
    /// </summary>
    public abstract class CircularMenuModel<T> : CircularMenuObjectModel where T : CircularMenuItem
    {
        /// <summary>
        /// Menu configuration.
        /// </summary>
        public CircularMenu<T> Config => menu;

        private readonly CircularMenu<T> menu;

        /// <summary>
        /// Rings.
        /// </summary>
        public IReadOnlyList<CircularMenuRingModel<T>> Rings => rings;

        private readonly List<CircularMenuRingModel<T>> rings = new List<CircularMenuRingModel<T>>();

        /// <summary>
        /// Initializes the menu.
        /// </summary>
        protected CircularMenuModel(CircularMenu<T> menu)
        {
            this.menu = menu;

            foreach (var item in menu.Items)
            {
                GetRing(item.Ring ?? 0).Add(item);
            }

            foreach (var ring in rings)
            {
                ring.ArrangeItems();
            }
        }

        /// <summary>
        /// Close the menu al free all elements.
        /// </summary>
        public void Close()
        {
            Release();
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            Release(rings);
            base.Release();
        }

        /// <summary>
        /// Finds the displayed menu item related to a menu item.
        /// </summary>
        public CircularMenuItemModel FindMenuItem(T item)
        {
            foreach (var ring in rings)
            {
                var di = ring.FindMenuItem(item);
                if (di != null)
                {
                    return di;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a specific ring.
        /// </summary>
        protected CircularMenuRingModel<T> GetRing(int ringIndex)
        {
            while (ringIndex >= rings.Count)
            {
                rings.Add(CreateRing());
            }

            return rings[ringIndex];
        }

        #region Abstract methods.

        /// <summary>
        /// Creates a ring.
        /// </summary>
        protected abstract CircularMenuRingModel<T> CreateRing();

        #endregion
    }
}
