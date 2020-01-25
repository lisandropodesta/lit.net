using System.Windows;
using System.Windows.Controls;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    public static class WpfCircularMenuTool
    {
        public static void Show(WpfRingSector ringSector, Canvas canvas, Point pos, CircularMenuObjectModel model)
        {
            ringSector.BeginUpdate();

            try
            {
                ringSector.Canvas = canvas;
                ringSector.CenterX = pos.X;
                ringSector.CenterY = pos.Y;
                ringSector.RadiusMin = model.FromRadius;
                ringSector.RadiusMax = model.ToRadius;
                ringSector.AngleFrom = model.FromAngle;
                ringSector.AngleTo = model.ToAngle;
            }
            finally
            {
                ringSector.EndUpdate();
            }
        }
    }
}
