using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu.
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

        #region Render

        /// <summary>
        /// Container canvas.
        /// </summary>
        public Canvas Canvas { get => canvas; private set => SetProp(ref canvas, value, Change.Visibility, nameof(Canvas)); }

        private Canvas canvas;

        /// <summary>
        /// Position at canvas.
        /// </summary>
        public Point Position { get => position; private set => SetProp(ref position, value, Change.Layout, nameof(Position)); }

        private Point position;

        /// <summary>
        /// Shows the menu.
        /// </summary>
        public void Show(Canvas canvas, Point pos)
        {
            BeginUpdate();

            IsShowing = false;

            Canvas = canvas;
            Position = pos;

            IsShowing = true;

            EndUpdate();
        }

        /// <summary>
        /// Hides the menu.
        /// </summary>
        public void Hide()
        {
            IsShowing = false;
        }

        /// <summary>
        /// Creates a ring.
        /// </summary>
        protected override CircularMenuRing<WpfCircularMenuItem> CreateRing()
        {
            return new WpfCircularMenuRing(this);
        }

        #endregion
    }
}
