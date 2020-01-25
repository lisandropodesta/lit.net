using System.Windows;
using System.Windows.Controls;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

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

        private readonly WpfRingSector ringSector = new WpfRingSector();

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuRingModel(WpfCircularMenuModel menu) : base(menu)
        {
            this.menu = menu;
        }

        /// <summary>
        /// Shows the ring.
        /// </summary>
        public void Show(Canvas canvas, Point pos)
        {
            WpfCircularMenuTool.Show(ringSector, canvas, pos, this);

            foreach (var item in Items)
            {
                (item as WpfCircularMenuItemModel).Show(canvas, pos);
            }
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            foreach (var item in Items)
            {
                item.Dispose();
            }

            ringSector.IsVisible = false;
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
