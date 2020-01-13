using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu model.
    /// </summary>
    public class WpfCircularMenuModel : CircularMenuModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuModel(Ui.CircularMenu.CircularMenu menu) : base(menu)
        {
        }

        protected override CircularMenuLayerModel CreateLayer()
        {
            return new WpfCircularMenuLayerModel(this);
        }
    }
}
