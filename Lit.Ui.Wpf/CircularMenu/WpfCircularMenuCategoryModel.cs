using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu category model.
    /// </summary>
    public class WpfCircularMenuCategoryModel : CircularMenuCategoryModel
    {
        /// <summary>
        /// Ring model.
        /// </summary>
        public WpfCircularMenuRingModel Ring => ring;

        private WpfCircularMenuRingModel ring;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuCategoryModel(WpfCircularMenuRingModel ring, string title) : base(title)
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
