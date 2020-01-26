using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        public bool IsVisible { get => isVisible; set => SetCanvasBindingProp(ref isVisible, value, nameof(IsVisible)); }

        private bool isVisible;

        /// <summary>
        /// Canvas control property.
        /// </summary>
        public Canvas Canvas { get => canvas; set => SetCanvasBindingProp(ref canvas, value, nameof(Canvas)); }

        private Canvas canvas;

        private bool inCanvas;

        /// <summary>
        /// Elements list.
        /// </summary>
        public IReadOnlyList<UIElement> UIElements => uiElements;

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

            if (IsDisplayed && inCanvas)
            {
                canvas.Children.Add(element);
            }
        }

        /// <summary>
        /// Removes an elemento from the shape.
        /// </summary>
        protected void RemoveElement(UIElement element)
        {
            if (uiElements.Contains(element))
            {
                if (inCanvas)
                {
                    canvas.Children.Remove(element);
                }

                uiElements.Remove(element);
            }
        }

        /// <summary>
        /// Update items.
        /// </summary>
        protected abstract void UpdateItems();

        /// <summary>
        /// Sets a property that makes necessary to remove/add elements to canvas.
        /// </summary>
        private bool SetCanvasBindingProp<T>(ref T prop, T value, string name = null)
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

                    InformPropertyChanged(Change.Visibility, name);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            base.OnPropertyChanged(change, name);

            if (change >= Change.Aspect)
            {
                UpdateItems();
            }

            if (change >= Change.Visibility)
            {
                if (IsDisplayed)
                {
                    AddElementsToCanvas();
                }
            }
        }

        /// <summary>
        /// Adds all elements to canvas.
        /// </summary>
        private void AddElementsToCanvas()
        {
            if (canvas != null && !inCanvas)
            {
                inCanvas = true;

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
            if (canvas != null && inCanvas)
            {
                foreach (var e in uiElements)
                {
                    canvas.Children.Remove(e);
                }

                inCanvas = false;
            }
        }
    }
}
