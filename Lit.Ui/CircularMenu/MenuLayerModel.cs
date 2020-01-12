using System;
using System.Collections.Generic;
using System.Linq;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu layer model.
    /// </summary>
    public abstract class MenuLayerModel : MenuObjectModel
    {
        /// <summary>
        /// Configuration.
        /// </summary>
        protected Menu Config => menu.Config;

        private readonly MenuModel menu;

        private readonly List<MenuItemModel> items = new List<MenuItemModel>();

        private readonly List<MenuCategoryModel> categories = new List<MenuCategoryModel>();

        private bool hasScrolling;

        /// <summary>
        /// Rotation angle.
        /// </summary>
        public override double Rotation
        {
            get => base.Rotation;
            set
            {
                if (base.Rotation != value)
                {
                    base.Rotation = value;
                    UpdateChildsRotation();
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MenuLayerModel(MenuModel menu)
        {
            this.menu = menu;
        }

        /// <summary>
        /// Adds an item to the layer.
        /// </summary>
        public void Add(MenuItem item)
        {
            var di = CreateItem(item);
            di.IsEnabled = item.IsEnabled;
            items.Add(di);
        }

        /// <summary>
        /// Finds the displayed menu item related to a menu item.
        /// </summary>
        public MenuItemModel FindMenuItem(MenuItem item)
        {
            return items.FirstOrDefault(i => i.Config == item);
        }

        /// <summary>
        /// Arranges all items.
        /// </summary>
        public void ArrangeItems()
        {
            if (Config.SortByCategory)
            {

            }

            CalcLayout();

            //SortByTargetAngle();

            CreateCategories();
        }

        /// <summary>
        /// Calculates item sizes and angles.
        /// </summary>
        private void CalcLayout()
        {
            double? fromAngle = null;
            var fromIndex = 0;
            var minRelSize = 1.0;
            var absSizeSum = 0.0;
            var relSizeSum = 0.0;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsVisible)
                {
                    var cfg = item.Config;
                    var absSize = 0.0;
                    var relSize = 0.0;

                    if (cfg.TargetSize.HasValue)
                    {
                        absSize = item.Config.TargetSize.Value;
                        absSizeSum += absSize;
                    }
                    else
                    {
                        relSize = GetItemRelativeSize(item);
                        minRelSize = Math.Min(minRelSize, relSize);
                        relSizeSum += relSize;
                    }

                    if (item.Config.TargetAngle.HasValue)
                    {
                        if (i == fromIndex)
                        {
                            item.RelAngle = item.Config.TargetAngle.Value;
                            item.Size = absSize > 0 ? absSize : Config.MinimumItemSize;
                        }
                        else
                        {
                            AssignLayout(fromIndex, i, fromAngle, item.Config.TargetAngle.Value, absSizeSum - absSize / 2, relSizeSum - relSize / 2, minRelSize);
                        }

                        fromAngle = item.ToAngle;
                        fromIndex = i + 1;
                        minRelSize = 1.0;
                        absSizeSum = 0.0;
                        relSizeSum = 0.0;
                    }
                }
            }

            if (fromIndex < items.Count)
            {
                AssignLayout(fromIndex, items.Count - 1, fromAngle, null, absSizeSum, relSizeSum, minRelSize);
            }

            if (hasScrolling)
            {
                UpdateChildsRotation();
            }
        }

        /// <summary>
        /// Assigns size and angle to a range of items.
        /// </summary>
        private void AssignLayout(int fromIndex, int toIndex, double? fromAngle, double? toAngle, double absSizeSum, double relSizeSum, double minRelSize)
        {
            // Calculates with available space
            var totalSize = (fromAngle ?? Config.DisplayFromAngle) - (toAngle ?? Config.DisplayToAngle);
            var relSizeValue = relSizeSum > 0 ? (totalSize - absSizeSum) / relSizeSum : 0.0;

            if (totalSize < absSizeSum || relSizeValue * minRelSize < Config.MinimumItemSize)
            {
                if (fromAngle.HasValue && toAngle.HasValue)
                {
                    throw new ArgumentException("Not enough space for menu items.");
                }

                // Calculates with scrolling
                hasScrolling = true;
                relSizeValue = relSizeSum > 0 ? Config.MinimumItemSize / minRelSize : 0.0;
                totalSize = absSizeSum + relSizeSum * relSizeValue;

                if (!fromAngle.HasValue)
                {
                    fromAngle = toAngle + totalSize;
                }
            }

            AssignLayout(fromIndex, toIndex, fromAngle.Value, relSizeValue);
        }

        /// <summary>
        /// Assigns size and angle to a range of items.
        /// </summary>
        private void AssignLayout(int fromIndex, int toIndex, double fromAngle, double relSizeValue)
        {
            var angle = fromAngle;
            for (var i = fromIndex; i <= toIndex; i++)
            {
                var item = items[i];
                if (item.IsVisible)
                {
                    var itemSize = GetItemAbsSize(item, relSizeValue);
                    item.Size = itemSize;
                    item.RelAngle = angle - itemSize / 2;
                    angle += itemSize;
                }
            }
        }

        /// <summary>
        /// Assign current rotation to all childs
        /// </summary>
        private void UpdateChildsRotation()
        {
            foreach (var item in items)
            {
                if (item.IsVisible)
                {
                    item.Rotation = Rotation;
                    item.IsHidden = item.FromAngle > Config.DisplayFromAngle || Config.DisplayToAngle > item.ToAngle;
                }
            }
        }

        /// <summary>
        /// Get the absolute item size.
        /// </summary>
        private static double GetItemAbsSize(MenuItemModel item, double relSizeValue)
        {
            if (item.Config.TargetSize.HasValue)
            {
                return item.Config.TargetSize.Value;
            }

            return relSizeValue * GetItemRelativeSize(item);
        }

        /// <summary>
        /// Force an item value of relative size.
        /// </summary>
        private static double GetItemRelativeSize(MenuItemModel item)
        {
            return !item.Config.RelativeSize.HasValue || item.Config.RelativeSize.Value <= 0 ? 1 : item.Config.RelativeSize.Value;
        }

        /// <summary>
        /// Create all categories.
        /// </summary>
        private void CreateCategories()
        {
            MenuItemModel prevItem = null;

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.Config.Category))
                {
                    if (prevItem?.Config.Category == item.Config.Category)
                    {
                        item.Category = prevItem.Category;
                        prevItem.LastCategoryObject = false;
                    }
                    else
                    {
                        item.Category = CreateCategory(item.Config.Category);
                        categories.Add(item.Category);

                        item.FirstCategoryObject = true;
                    }

                    item.LastCategoryObject = true;
                }

                prevItem = item;
            }
        }

        #region Abstract methods

        /// <summary>
        /// Creates an item.
        /// </summary>
        protected abstract MenuItemModel CreateItem(MenuItem item);

        /// <summary>
        /// Creates a category.
        /// </summary>
        protected abstract MenuCategoryModel CreateCategory(string title);

        #endregion
    }
}
