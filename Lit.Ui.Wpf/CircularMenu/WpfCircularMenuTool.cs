using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// Extension methods helper.
    /// </summary>
    public static class WpfCircularMenuTool
    {
        public static void Show(this WpfRingSector ringSector, WpfCircularMenuItem model, Canvas canvas, Point pos)
        {
            ringSector.Show(model as CircularMenuObjectModel, canvas, pos, model.ShapeBorderThickness, model.ShapeBorderColor, model.ShapeBackgroundColor);
        }

        public static void Update(this WpfRingSector ringSector, WpfCircularMenuItem model)
        {
            ringSector.Update(model as CircularMenuObjectModel, model.ShapeBorderThickness, model.ShapeBorderColor, model.ShapeBackgroundColor);
        }

        public static void Show(this WpfRingSector ringSector, CircularMenuObjectModel model, Canvas canvas, Point pos, double? borderThickness = null, Color? borderColor = null, Color? backgroundColor = null)
        {
            ringSector.BeginUpdate();

            try
            {
                ringSector.Canvas = canvas;
                ringSector.CenterX = pos.X;
                ringSector.CenterY = pos.Y;
                ringSector.Update(model, borderThickness, borderColor, backgroundColor);
            }
            finally
            {
                ringSector.EndUpdate();
            }
        }

        public static void Update(this WpfRingSector ringSector, CircularMenuObjectModel model, double? borderThickness = null, Color? borderColor = null, Color? backgroundColor = null)
        {
            var mustDisplay = model.MustDisplay;

            if (mustDisplay || ringSector.IsDisplayed)
            {
                ringSector.BeginUpdate();

                try
                {
                    ringSector.IsVisible = mustDisplay;

                    if (mustDisplay)
                    {
                        ringSector.RadiusMin = model.FromRadius;
                        ringSector.RadiusMax = model.ToRadius;
                        ringSector.AngleFrom = model.FromAngle;
                        ringSector.AngleTo = model.ToAngle;
                        ringSector.BorderThickness = borderThickness ?? 1;
                        ringSector.BorderColor = borderColor ?? Colors.Black;
                        ringSector.BackgroundColor = backgroundColor ?? Colors.White;
                    }
                }
                finally
                {
                    ringSector.EndUpdate();
                }
            }
        }
    }
}
