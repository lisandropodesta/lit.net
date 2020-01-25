using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Lit.Ui.Classes;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// WPF shape base.
    /// </summary>
    public abstract class WpfShapeBase : BaseModel
    {
        /// <summary>
        /// Displayed flag.
        /// </summary>
        public bool IsDisplayed => isVisible && canvas != null;

        /// <summary>
        /// Visibility control property.
        /// </summary>
        public bool IsVisible { get => isVisible; set => SetCanvasBindingProp(ref isVisible, value, nameof(IsVisible), false); }

        private bool isVisible;

        /// <summary>
        /// Canvas control property.
        /// </summary>
        public Canvas Canvas { get => canvas; set => SetCanvasBindingProp(ref canvas, value, nameof(Canvas), false); }

        private Canvas canvas;

        /// <summary>
        /// Elements list.
        /// </summary>
        protected IReadOnlyList<UIElement> UIElements => uiElements;

        private readonly List<UIElement> uiElements = new List<UIElement>();

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WpfShapeBase()
        {
            isVisible = true;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        protected override void Release()
        {
            if (isVisible)
            {
                RemoveElementsFromCanvas();
            }

            uiElements.Clear();
            canvas = null;

            base.Release();
        }

        /// <summary>
        /// Adds an element to the shape.
        /// </summary>
        protected void AddElement(UIElement element)
        {
            uiElements.Add(element);

            if (IsDisplayed)
            {
                canvas.Children.Add(element);
            }
        }

        /// <summary>
        /// Removes an elemento from the shape.
        /// </summary>
        /// <param name="element"></param>
        protected void RemoveElement(UIElement element)
        {
            if (uiElements.Contains(element))
            {
                uiElements.Remove(element);

                if (IsDisplayed)
                {
                    canvas.Children.Remove(element);
                }
            }
        }

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnLayoutChanged()
        {
            base.OnLayoutChanged();

            UpdateItems();
        }

        /// <summary>
        /// Update items.
        /// </summary>
        protected abstract void UpdateItems();

        /// <summary>
        /// Sets a property that makes necessary to remove/add elements to canvas.
        /// </summary>
        private bool SetCanvasBindingProp<T>(ref T prop, T value, string name = null, bool layoutChanged = false)
        {
            lock (this)
            {
                if (prop == null && value != null || prop != null && !prop.Equals(value))
                {
                    if (isVisible)
                    {
                        RemoveElementsFromCanvas();
                    }

                    prop = value;

                    if (isVisible)
                    {
                        AddElementsToCanvas();
                    }

                    OnPropertyChanged(name, layoutChanged);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Adds all elements to canvas.
        /// </summary>
        private void AddElementsToCanvas()
        {
            if (canvas != null)
            {
                foreach (var e in uiElements)
                {
                    canvas.Children.Add(e);
                }
            }
        }

        /// <summary>
        /// Removes all elements from canvas.
        /// </summary>
        private void RemoveElementsFromCanvas()
        {
            if (canvas != null)
            {
                foreach (var e in uiElements)
                {
                    canvas.Children.Remove(e);
                }
            }
        }
    }
}
