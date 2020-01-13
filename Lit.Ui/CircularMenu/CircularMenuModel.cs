using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu model.
    /// </summary>
    public abstract class CircularMenuModel : CircularMenuObjectModel
    {
        /// <summary>
        /// Menu configuration.
        /// </summary>
        public CircularMenu Config => menu;

        private readonly CircularMenu menu;

        /// <summary>
        /// Layers.
        /// </summary>
        public IReadOnlyList<CircularMenuLayerModel> Layers => layers;

        private readonly List<CircularMenuLayerModel> layers = new List<CircularMenuLayerModel>();

        /// <summary>
        /// Initializes the menu.
        /// </summary>
        protected CircularMenuModel(CircularMenu menu)
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
        /// Finds the displayed menu item related to a menu item.
        /// </summary>
        public CircularMenuItemModel FindMenuItem(CircularMenuItem item)
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
        protected CircularMenuLayerModel GetLayer(int layer)
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
        protected abstract CircularMenuLayerModel CreateLayer();

        #endregion
    }
}
