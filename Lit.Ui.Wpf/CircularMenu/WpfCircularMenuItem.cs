using System;
using System.Windows;
using System.Windows.Media;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item.
    /// </summary>
    public class WpfCircularMenuItem : CircularMenuItem
    {
        /// <summary>
        /// Shaper background color.
        /// </summary>
        public Color? ShapeBackgroundColor { get; set; }

        /// <summary>
        /// Shape border color.
        /// </summary>
        public Color? ShapeBorderColor { get; set; }

        /// <summary>
        /// Action to be executed.
        /// </summary>
        public Action<WpfCircularMenuItem, object, object> Command { get; set; }

        /// <summary>
        /// Command parameter.
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// Triggers the action
        /// </summary>
        public override void TriggerAction(object sender)
        {
            base.TriggerAction(sender);

            Command.Invoke(this, sender, CommandParameter);
        }

        #region Render

        /// <summary>
        /// Assigned ring.
        /// </summary>
        public WpfCircularMenuRing WpfRing { get => wpfRing; set => SetProp(ref wpfRing, value, Change.Visibility); }

        private WpfCircularMenuRing wpfRing;

        private readonly WpfRingSector ringSector = new WpfRingSector();

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            base.OnPropertyChanged(change, name);

            if (change >= Change.Visibility)
            {
                ringSector.Show(this, wpfRing?.WpfMenu.Canvas, wpfRing?.WpfMenu.Position ?? new Point());
            }
        }

        #endregion
    }
}
