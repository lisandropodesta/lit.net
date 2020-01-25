using System.Windows;
using System.Windows.Controls;
using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu model.
    /// </summary>
    public class WpfCircularMenuModel : CircularMenuModel<WpfCircularMenuItem>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuModel(CircularMenu<WpfCircularMenuItem> menu) : base(menu)
        {
        }

        /// <summary>
        /// Shows the menu.
        /// </summary>
        public void Show(Canvas canvas, Point pos)
        {
            foreach (var ring in Rings)
            {
                (ring as WpfCircularMenuRingModel).Show(canvas, pos);
            }
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            base.Release();
        }

        protected override CircularMenuRingModel<WpfCircularMenuItem> CreateRing()
        {
            return new WpfCircularMenuRingModel(this);
        }
    }
}
