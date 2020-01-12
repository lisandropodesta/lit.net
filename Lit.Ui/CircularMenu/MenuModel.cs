using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu model.
    /// </summary>
    public abstract class MenuModel : MenuObjectModel
    {
        /// <summary>
        /// Menu configuration.
        /// </summary>
        public Menu Config => menu;

        private readonly Menu menu;

        /// <summary>
        /// Layers.
        /// </summary>
        public IReadOnlyList<MenuLayerModel> Layers => layers;

        private readonly List<MenuLayerModel> layers = new List<MenuLayerModel>();

        /// <summary>
        /// Initializes the menu.
        /// </summary>
        protected MenuModel(Menu menu)
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
        public MenuItemModel FindMenuItem(MenuItem item)
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
        protected MenuLayerModel GetLayer(int layer)
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
        protected abstract MenuLayerModel CreateLayer();

        #endregion
    }
}
