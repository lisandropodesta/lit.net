using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item model.
    /// </summary>
    public class WpfMenuItemModel : MenuItemModel
    {
        /// <summary>
        /// Layer model.
        /// </summary>
        public WpfMenuLayerModel Layer => layer;

        private readonly WpfMenuLayerModel layer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfMenuItemModel(WpfMenuLayerModel layer, MenuItem item) : base(item)
        {
            this.layer = layer;
        }
    }
}
