using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu ring model.
    /// </summary>
    public class WpfCircularMenuRingModel : CircularMenuRingModel<WpfCircularMenuItem>
    {
        /// <summary>
        /// Menu model.
        /// </summary>
        public WpfCircularMenuModel Menu => menu;

        private WpfCircularMenuModel menu;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuRingModel(WpfCircularMenuModel menu) : base(menu)
        {
            this.menu = menu;
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            menu = null;
            base.Release();
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
