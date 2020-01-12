using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Lit.Ui.Wpf.Animation;

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
        }
    }
}
