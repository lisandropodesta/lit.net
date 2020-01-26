namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu item.
    /// </summary>
    public class CircularMenuItem : CircularMenuObjectModel
    {
        /// <summary>
        /// Item category.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Item ring.
        /// </summary>
        public int? Ring { get; set; }

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
        /// Shape border thickness.
        /// </summary>
        public double? ShapeBorderThickness { get; set; }

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

        #region Render

        /// <summary>
        /// Associated category.
        /// </summary>
        public CircularMenuCategory Category { get; set; }

        /// <summary>
        /// Flag if this is the first object on the category (clockwise).
        /// </summary>
        public bool FirstCategoryObject { get; set; }

        /// <summary>
        /// Flag if this is the last object on the category (clockwise).
        /// </summary>
        public bool LastCategoryObject { get; set; }

        /// <summary>
        /// Process layout changes.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            base.OnPropertyChanged(change, name);

            if (change >= Change.Layout && Category != null)
            {
                if (FirstCategoryObject)
                {
                    Category.FromAngle = FromAngle;
                }

                if (LastCategoryObject)
                {
                    Category.ToAngle = ToAngle;
                }
            }
        }

        #endregion
    }
}
