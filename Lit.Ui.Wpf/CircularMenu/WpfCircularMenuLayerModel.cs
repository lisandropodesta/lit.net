using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu layer model.
    /// </summary>
    public class WpfCircularMenuLayerModel : CircularMenuLayerModel
    {
        /// <summary>
        /// Menu model.
        /// </summary>
        public WpfCircularMenuModel Menu => menu;

        private readonly WpfCircularMenuModel menu;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuLayerModel(WpfCircularMenuModel menu) : base(menu)
        {
            this.menu = menu;
        }

        protected override CircularMenuCategoryModel CreateCategory(string title)
        {
            return new WpfCircularMenuCategoryModel(this, title);
        }

        protected override CircularMenuItemModel CreateItem(CircularMenuItem item)
        {
            return new WpfCircularMenuItemModel(this, item);
        }
    }
}
