using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu category model.
    /// </summary>
    public class WpfMenuCategoryModel : MenuCategoryModel
    {
        /// <summary>
        /// Layer model.
        /// </summary>
        public WpfMenuLayerModel Layer => layer;

        private readonly WpfMenuLayerModel layer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfMenuCategoryModel(WpfMenuLayerModel layer, string title) : base(title)
        {
            this.layer = layer;
        }
    }
}
