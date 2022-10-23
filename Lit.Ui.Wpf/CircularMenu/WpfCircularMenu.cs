using System.Windows;
using System.Windows.Controls;
using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu.
    /// </summary>
    public class WpfCircularMenu : Lit.Ui.CircularMenu.CircularMenu
    {
        /// <summary>
        /// Container canvas.
        /// </summary>
        internal Canvas Canvas { get => canvas; private set => SetProp(ref canvas, value, Change.Visibility); }

        private Canvas canvas;

        /// <summary>
        /// Position at canvas.
        /// </summary>
        internal Point Position { get => position; private set => SetProp(ref position, value, Change.Layout); }

        private Point position;

        /// <summary>
        /// Shows the menu.
        /// </summary>
        public void ShowAt(Canvas canvas, Point pos)
        {
            BeginChange();

            base.Show = false;

            Canvas = canvas;
            Position = pos;

            base.Show = true;

            EndChange();
        }

        /// <summary>
        /// Creates a ring.
        /// </summary>
        protected override CircularMenuRing CreateRing()
        {
            return new WpfCircularMenuRing(this);
        }
    }
}
