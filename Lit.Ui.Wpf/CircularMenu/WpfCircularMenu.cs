using Lit.Ui.CircularMenu;
using System.Windows.Media;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu configuration.
    /// </summary>
    public class WpfCircularMenu : CircularMenu<WpfCircularMenuItem>
    {
        /// <summary>
        /// Default background color for items.
        /// </summary>
        public Color ItemsDefaultBackground { get; set; }

        /// <summary>
        /// Default border color for items.
        /// </summary>
        public Color ItemsDefaultBorder { get; set; }
    }
}
