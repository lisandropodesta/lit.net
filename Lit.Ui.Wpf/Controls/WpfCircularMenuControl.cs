using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.CircularMenu;

namespace Lit.Ui.Wpf.Controls
{
    /// <summary>
    /// Awesome circular menu.
    /// </summary>
    public class WpfCircularMenuControl : Canvas
    {
        static WpfCircularMenuControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfCircularMenuControl), new FrameworkPropertyMetadata(typeof(WpfCircularMenuControl)));
        }

        private WpfCircularMenu menu;

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuControl()
        {
            this.Loaded += CircularMenu_Loaded;
        }

        private void CircularMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.MouseDown += Window_MouseDown;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this);

            var target = GetTarget(e.Source);
            if (target != null)
            {
                CloseMenu();

                menu = target.ContextMenu as WpfCircularMenu;
                menu.ShowAt(this, pos);
            }
        }

        private void CloseMenu()
        {
            if (menu != null)
            {
                menu.Show = false;
                menu = null;
            }
        }

        private ICircularMenuContext GetTarget(object source)
        {
            while (source != null)
            {
                if (source is ICircularMenuContext menu)
                {
                    return menu;
                }

                source = (source as FrameworkElement)?.Parent;
            }

            return null;
        }
    }
}
