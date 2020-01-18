using Lit.Ui.CircularMenu;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuItemModel(WpfCircularMenuRingModel ring, CircularMenuItem item) : base(item)
        {
            this.ring = ring;
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            ring = null;
            base.Release();
        }
    }
}
