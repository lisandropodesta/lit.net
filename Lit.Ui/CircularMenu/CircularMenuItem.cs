namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu item configuration.
    /// </summary>
    public class CircularMenuItem
    {
        /// <summary>
        /// Item category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Item ring.
        /// </summary>
        public int? Ring { get; set; }

        /// <summary>
        /// Visible flag.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Enabled flag.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Short text to be displayed.
        /// </summary>
        public string ShortText { get; set; }

        /// <summary>
        /// Text to be displayed.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Long text to be displayed.
        /// </summary>
        public string LongText { get; set; }

        /// <summary>
        /// Description of the option.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Icon.
        /// </summary>
        public object Icon { get; set; }

        /// <summary>
        /// Display shape.
        /// </summary>
        public CircularItemShape Shape { get; set; }

        /// <summary>
        /// Target angle (radians).
        /// </summary>
        public double? TargetAngle { get; set; }

        /// <summary>
        /// Target radial size.
        /// </summary>
        public double? TargetSize { get; set; }

        /// <summary>
        /// Relative size to other items.
        /// </summary>
        public double? RelativeSize { get; set; }

        /// <summary>
        /// Triggers the action
        /// </summary>
        public virtual void TriggerAction(object sender)
        {
        }
    }
}
