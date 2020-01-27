using System;
using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu ring.
    /// </summary>
    public abstract class CircularMenuRing : CircularMenuObjectModel
    {
        /// <summary>
        /// Configuration.
        /// </summary>
        protected CircularMenu Menu { get; private set; }

        /// <summary>
        /// Items.
        /// </summary>
        public IReadOnlyList<CircularMenuItem> Items => items;

        private readonly List<CircularMenuItem> items = new List<CircularMenuItem>();

        private readonly List<CircularMenuCategory> categories = new List<CircularMenuCategory>();

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
        protected CircularMenuRing(CircularMenu menu)
        {
            Menu = menu;
        }

        /// <summary>
        /// Release all memory references.
        /// </summary>
        protected override void Release()
        {
            Release(items);
            Release(categories);
            base.Release();
        }

        /// <summary>
        /// Clear al items and categories.
        /// </summary>
        internal void ClearItems()
        {
            items.Clear();
            categories.Clear();
        }

        /// <summary>
        /// Adds an item to the ring.
        /// </summary>
        internal void Add(CircularMenuItem item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Arranges all items.
        /// </summary>
        internal void ArrangeItems()
        {
            if (Menu.SortByCategory)
            {
                // TODO: implement this feature
            }

            CalcItemsLayout();

            CalcLayout();

            CreateCategories();
        }

        /// <summary>
        /// Calculates the ring limits.
        /// </summary>
        private void CalcLayout()
        {
            if (FromAngle == ToAngle)
            {
                var fromAngle = 5 * Math.PI / 4;
                var toAngle = -Math.PI / 4;

                if (!hasScrolling)
                {
                    var first = true;
                    foreach (var item in items)
                    {
                        if (first || fromAngle < item.FromAngle)
                        {
                            fromAngle = item.FromAngle;
                        }

                        if (first || toAngle > item.ToAngle)
                        {
                            toAngle = item.ToAngle;
                        }

                        first = false;
                    }
                }

                FromAngle = fromAngle;
                ToAngle = toAngle;
            }
        }

        /// <summary>
        /// Calculates item sizes and angles.
        /// </summary>
        private void CalcItemsLayout()
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
                    var cfg = item;
                    var absSize = 0.0;
                    var relSize = 0.0;

                    if (cfg.TargetSize.HasValue)
                    {
                        absSize = cfg.TargetSize.Value;
                        absSizeSum += absSize;
                    }
                    else
                    {
                        relSize = GetItemRelativeSize(item);
                        minRelSize = Math.Min(minRelSize, relSize);
                        relSizeSum += relSize;
                    }

                    if (cfg.TargetAngle.HasValue)
                    {
                        if (i > fromIndex)
                        {
                            AssignLayout(fromIndex, i, fromAngle, cfg.TargetAngle.Value, absSizeSum - absSize / 2, relSizeSum - relSize / 2, minRelSize);
                        }
                        else
                        {
                            item.RelAngle = cfg.TargetAngle.Value;
                            item.Size = absSize > 0 ? absSize : Menu.MinimumItemSize;
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
            var totalSize = (fromAngle ?? Menu.DisplayFromAngle) - (toAngle ?? Menu.DisplayToAngle);
            var relSizeValue = relSizeSum > 0 ? (totalSize - absSizeSum) / relSizeSum : 0.0;

            if (totalSize < absSizeSum || relSizeValue * minRelSize < Menu.MinimumItemSize)
            {
                if (fromAngle.HasValue && toAngle.HasValue)
                {
                    throw new ArgumentException("Not enough space for menu items.");
                }

                // Calculates with scrolling
                hasScrolling = true;
                relSizeValue = relSizeSum > 0 ? Menu.MinimumItemSize / minRelSize : 0.0;
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
                    angle -= itemSize;
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
                    item.IsHidden = item.FromAngle > Menu.DisplayFromAngle || Menu.DisplayToAngle > item.ToAngle;
                }
            }
        }

        /// <summary>
        /// Get the absolute item size.
        /// </summary>
        private static double GetItemAbsSize(CircularMenuItem item, double relSizeValue)
        {
            if (item.TargetSize.HasValue)
            {
                return item.TargetSize.Value;
            }

            return relSizeValue * GetItemRelativeSize(item);
        }

        /// <summary>
        /// Force an item value of relative size.
        /// </summary>
        private static double GetItemRelativeSize(CircularMenuItem item)
        {
            return !item.RelativeSize.HasValue || item.RelativeSize.Value <= 0 ? 1 : item.RelativeSize.Value;
        }

        /// <summary>
        /// Create all categories.
        /// </summary>
        private void CreateCategories()
        {
            CircularMenuItem prevItem = null;

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.CategoryName))
                {
                    if (prevItem?.CategoryName == item.CategoryName)
                    {
                        item.CategoryName = prevItem.CategoryName;
                        prevItem.LastCategoryObject = false;
                    }
                    else
                    {
                        item.Category = CreateCategory(item.CategoryName);
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
        /// Creates a category.
        /// </summary>
        protected abstract CircularMenuCategory CreateCategory(string title);

        #endregion
    }
}
