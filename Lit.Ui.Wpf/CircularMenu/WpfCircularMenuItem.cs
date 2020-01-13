using System;
using System.Windows.Media;
using Lit.Ui.CircularMenu;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item configuration.
    /// </summary>
    public class WpfCircularMenuItem : CircularMenuItem
    {
        /// <summary>
        /// Shaper background color.
        /// </summary>
        public Color? ShapeBackground { get; set; }

        /// <summary>
        /// Shape border color.
        /// </summary>
        public Color? ShapeBorder { get; set; }

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
    }
}
