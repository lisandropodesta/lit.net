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
        /// Layers.
        /// </summary>
        public IReadOnlyList<CircularMenuLayerModel<T>> Layers => layers;

        private readonly List<CircularMenuLayerModel<T>> layers = new List<CircularMenuLayerModel<T>>();

        /// <summary>
        /// Initializes the menu.
        /// </summary>
        protected CircularMenuModel(CircularMenu<T> menu)
        {
            this.menu = menu;

            foreach (var item in menu.Items)
            {
                GetLayer(item.Layer ?? 0).Add(item);
            }

            foreach (var layer in layers)
            {
                layer.ArrangeItems();
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
            Release(layers);
            base.Release();
        }

        /// <summary>
        /// Finds the displayed menu item related to a menu item.
        /// </summary>
        public CircularMenuItemModel FindMenuItem(T item)
        {
            foreach (var layer in layers)
            {
                var di = layer.FindMenuItem(item);
                if (di != null)
                {
                    return di;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a specific layer.
        /// </summary>
        protected CircularMenuLayerModel<T> GetLayer(int layer)
        {
            while (layer >= layers.Count)
            {
                layers.Add(CreateLayer());
            }

            return layers[layer];
        }

        #region Abstract methods.

        /// <summary>
        /// Creates a layer.
        /// </summary>
        protected abstract CircularMenuLayerModel<T> CreateLayer();

        #endregion
    }
}
