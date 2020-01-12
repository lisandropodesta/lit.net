﻿namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu item model.
    /// </summary>
    public abstract class MenuItemModel : MenuObjectModel
    {
        /// <summary>
        /// Item definition.
        /// </summary>
        public MenuItem Config { get => item; private set => SetProp(ref item, value); }

        private MenuItem item;

        /// <summary>
        /// Enabled flag.
        /// </summary>
        public bool IsEnabled { get => isEnabled; set => SetProp(ref isEnabled, value, nameof(IsEnabled)); }

        private bool isEnabled;

        /// <summary>
        /// Associated category.
        /// </summary>
        public MenuCategoryModel Category { get; set; }

        /// <summary>
        /// Flag if this is the first object on the category (clockwise).
        /// </summary>
        public bool FirstCategoryObject { get; set; }

        /// <summary>
        /// Flag if this is the last object on the category (clockwise).
        /// </summary>
        public bool LastCategoryObject { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MenuItemModel(MenuItem item)
        {
            this.item = item;
        }

        /// <summary>
        /// Process layout changes.
        /// </summary>
        protected override void OnLayoutChanged()
        {
            base.OnLayoutChanged();

            if (Category != null)
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
    }
}
