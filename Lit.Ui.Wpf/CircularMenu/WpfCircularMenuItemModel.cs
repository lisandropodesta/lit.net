using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item model.
    /// </summary>
    public class WpfCircularMenuItemModel : CircularMenuItemModel
    {
        /// <summary>
        /// Layer model.
        /// </summary>
        public WpfCircularMenuLayerModel Layer => layer;

        private WpfCircularMenuLayerModel layer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuItemModel(WpfCircularMenuLayerModel layer, CircularMenuItem item) : base(item)
        {
            this.layer = layer;
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            layer = null;
            base.Release();
        }
    }
}
