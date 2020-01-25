using System.Windows;
using System.Windows.Controls;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item model.
    /// </summary>
    public class WpfCircularMenuItemModel : CircularMenuItemModel
    {
        /// <summary>
        /// Ring model.
        /// </summary>
        public WpfCircularMenuRingModel Ring => ring;

        private WpfCircularMenuRingModel ring;

        private readonly WpfRingSector ringSector = new WpfRingSector();

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuItemModel(WpfCircularMenuRingModel ring, CircularMenuItem item) : base(item)
        {
            this.ring = ring;
        }

        /// <summary>
        /// Shows the item.
        /// </summary>
        public void Show(Canvas canvas, Point pos)
        {
            WpfCircularMenuTool.Show(ringSector, canvas, pos, this);
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            ringSector.IsVisible = false;
            ring = null;
            base.Release();
        }
    }
}
