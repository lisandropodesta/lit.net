using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu layer model.
    /// </summary>
    public class WpfMenuLayerModel : MenuLayerModel
    {
        /// <summary>
        /// Menu model.
        /// </summary>
        public WpfMenuModel Menu => menu;

        private readonly WpfMenuModel menu;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfMenuLayerModel(WpfMenuModel menu) : base(menu)
        {
            this.menu = menu;
        }

        protected override MenuCategoryModel CreateCategory(string title)
        {
            return new WpfMenuCategoryModel(this, title);
        }

        protected override MenuItemModel CreateItem(MenuItem item)
        {
            return new WpfMenuItemModel(this, item);
        }
    }
}
