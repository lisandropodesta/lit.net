using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu category model.
    /// </summary>
    public class WpfCircularMenuCategoryModel : CircularMenuCategoryModel
    {
        /// <summary>
        /// Layer model.
        /// </summary>
        public WpfCircularMenuLayerModel Layer => layer;

        private WpfCircularMenuLayerModel layer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuCategoryModel(WpfCircularMenuLayerModel layer, string title) : base(title)
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
