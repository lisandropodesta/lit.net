using Lit.Ui.CircularMenu;
using System.Windows.Controls;

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
        public void Show(Panel container)
        {
            throw new System.NotImplementedException();
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
