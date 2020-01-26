using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu category.
    /// </summary>
    public class WpfCircularMenuCategory : CircularMenuCategory
    {
        /// <summary>
        /// Ring model.
        /// </summary>
        public WpfCircularMenuRing Ring => ring;

        private WpfCircularMenuRing ring;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuCategory(WpfCircularMenuRing ring, string title) : base(title)
        {
            this.ring = ring;
        }
    }
}
