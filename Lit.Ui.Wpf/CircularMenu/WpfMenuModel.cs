using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu model.
    /// </summary>
    public class WpfMenuModel : MenuModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfMenuModel(Menu menu) : base(menu)
        {
        }

        protected override MenuLayerModel CreateLayer()
        {
            return new WpfMenuLayerModel(this);
        }
    }
}
