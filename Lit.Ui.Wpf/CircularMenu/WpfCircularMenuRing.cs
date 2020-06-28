using System.Windows.Controls;
using System.Windows.Media;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu ring.
    /// </summary>
    public class WpfCircularMenuRing : CircularMenuRing, IWpfRingSectorSource
    {
        internal WpfCircularMenu WpfMenu => wpfMenu;

        private readonly WpfCircularMenu wpfMenu;

        private readonly WpfRingSector ringSector = new WpfRingSector();

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuRing(WpfCircularMenu menu) : base(menu)
        {
            wpfMenu = menu;
        }

        #region IWpfRingSectorSource

        bool IWpfShapeSource.IsVisible => IsShowing;

        Canvas IWpfShapeSource.Canvas => WpfMenu.Canvas;

        double IWpfRingSectorSource.CenterX => wpfMenu.Position.X;

        double IWpfRingSectorSource.CenterY => wpfMenu.Position.Y;

        double IWpfRingSectorSource.RadiusMin => FromRadius;

        double IWpfRingSectorSource.RadiusMax => ToRadius;

        double IWpfRingSectorSource.AngleFrom => FromAngle;

        double IWpfRingSectorSource.AngleTo => ToAngle;

        double IWpfRingSectorSource.BorderThickness => 1;

        Color IWpfRingSectorSource.BorderColor => Colors.Black;

        Color IWpfRingSectorSource.BackgroundColor => Colors.White;

        #endregion

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            base.OnPropertyChanged(change, name);

            if (change >= Change.Aspect)
            {
                ringSector.Update(this);

                Items.ForEach(i => i.BeginChange());

                Items.ForEach(item =>
                {
                    (item as WpfCircularMenuItem).WpfRing = this;
                    item.Show = Show;
                    item.FromRadius = FromRadius;
                    item.ToRadius = ToRadius;
                });

                Items.ForEach(i => i.EndChange());
            }
        }

        /// <summary>
        /// Creates a category.
        /// </summary>
        protected override CircularMenuCategory CreateCategory(string title)
        {
            return new WpfCircularMenuCategory(this, title);
        }
    }
}
