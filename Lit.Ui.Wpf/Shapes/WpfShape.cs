using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// WPF shape base.
    /// </summary>
    public abstract class WpfShape<T> where T : IWpfShapeSource
    {
        /// <summary>
        /// Displayed flag.
        /// </summary>
        public bool IsDisplayed => isDisplayed;

        private bool isDisplayed;

        private Canvas canvas;

        /// <summary>
        /// Elements list.
        /// </summary>
        public IReadOnlyList<UIElement> UIElements => uiElements;

        private readonly List<UIElement> uiElements = new List<UIElement>();

        /// <summary>
        /// Updates the shape.
        /// </summary>
        public void Update(T source)
        {
            lock (this)
            {
                if (isDisplayed && (canvas != source.Canvas || !source.IsVisible))
                {
                    uiElements.ForEach(e => canvas.Children.Remove(e));
                    canvas = null;
                    isDisplayed = false;
                }

                if (source.IsVisible && (canvas = source.Canvas) != null)
                {
                    UpdateItems(source);

                    if (!isDisplayed)
                    {
                        uiElements.ForEach(e => canvas.Children.Add(e));
                        isDisplayed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Update items.
        /// </summary>
        protected abstract void UpdateItems(T source);

        /// <summary>
        /// Adds an element to the shape.
        /// </summary>
        protected void AddElement(UIElement element)
        {
            uiElements.Add(element);

            if (isDisplayed)
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
                if (isDisplayed)
                {
                    canvas.Children.Remove(element);
                }

                uiElements.Remove(element);
            }
        }
    }
}
