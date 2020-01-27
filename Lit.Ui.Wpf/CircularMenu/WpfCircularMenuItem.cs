using System.Windows.Controls;
using System.Windows.Media;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu item.
    /// </summary>
    public class WpfCircularMenuItem : CircularMenuItem, IWpfRingSectorSource
    {
        #region IWpfRingSectorSource

        bool IWpfShapeSource.IsVisible => IsShowing;

        Canvas IWpfShapeSource.Canvas => WpfRing?.WpfMenu.Canvas;

        double IWpfRingSectorSource.CenterX => WpfRing?.WpfMenu.Position.X ?? 0;

        double IWpfRingSectorSource.CenterY => WpfRing?.WpfMenu.Position.Y ?? 0;

        double IWpfRingSectorSource.RadiusMin => FromRadius;

        double IWpfRingSectorSource.RadiusMax => ToRadius;

        double IWpfRingSectorSource.AngleFrom => FromAngle;

        double IWpfRingSectorSource.AngleTo => ToAngle;

        double IWpfRingSectorSource.BorderThickness => ShapeBorderThickness ?? 1;

        Color IWpfRingSectorSource.BorderColor => ShapeBorderColor.HasValue ? ShapeBorderColor.Value.WpfColor() : Colors.Black;

        Color IWpfRingSectorSource.BackgroundColor => ShapeBackgroundColor.HasValue ? ShapeBackgroundColor.Value.WpfColor() : Colors.White;

        #endregion

        #region Render

        /// <summary>
        /// Assigned ring.
        /// </summary>
        internal WpfCircularMenuRing WpfRing { get => wpfRing; set => SetProp(ref wpfRing, value, Change.Visibility); }

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
                var first = ringSector.Path == null;

                ringSector.Update(this);

                if (first && ringSector.Path != null && Command != null)
                {
                    ringSector.Path.MouseLeftButtonUp += (s, e) => TriggerAction();
                }
            }
        }

        #endregion
    }
}
