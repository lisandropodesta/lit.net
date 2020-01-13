using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lit.Ui.Wpf.CircularMenu;

namespace Lit.Ui.Wpf.Controls
{
    /// <summary>
    /// Awesome circular menu.
    /// </summary>
    public class CircularMenuControl : Canvas
    {
        static CircularMenuControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularMenuControl), new FrameworkPropertyMetadata(typeof(CircularMenuControl)));
        }

        private WpfCircularMenuModel model;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CircularMenuControl()
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
            var p = e.GetPosition(this);

            var target = GetTarget(e.Source);
            if (target != null)
            {
                CloseMenu();

                model = new WpfCircularMenuModel(target.ContextMenu);
                model.Show(this);
            }
        }

        private void CloseMenu()
        {
            if (model != null)
            {
                model.Close();
                model = null;
            }
        }

        private IWpfCircularMenuContext GetTarget(object source)
        {
            while (source != null)
            {
                if (source is IWpfCircularMenuContext menu)
                {
                    return menu;
                }

                source = (source as FrameworkElement)?.Parent;
            }

            return null;
        }
    }
}
