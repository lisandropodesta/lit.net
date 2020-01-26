using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// WPF ring sector shape.
    /// </summary>
    public class WpfRingSector : WpfShapeBase<IWpfRingSectorSource>
    {
        /// <summary>
        /// Generated path.
        /// </summary>
        public Path Path => path;

        private Path path;

        /// <summary>
        /// Updates the shape.
        /// </summary>
        protected override void UpdateItems(IWpfRingSectorSource source)
        {
            var centerX = source.CenterX;
            var centerY = source.CenterY;
            var radiusMin = source.RadiusMin;
            var radiusMax = source.RadiusMax;
            var angleFrom = source.AngleFrom;
            var angleTo = source.AngleTo;

            var cos = Math.Cos(angleFrom);
            var sin = Math.Sin(angleFrom);

            var p0 = new Point { X = centerX + radiusMin * cos, Y = centerY - radiusMin * sin };
            var p1 = new Point { X = centerX + radiusMax * cos, Y = centerY - radiusMax * sin };

            cos = Math.Cos(angleTo);
            sin = Math.Sin(angleTo);

            var p2 = new Point { X = centerX + radiusMax * cos, Y = centerY - radiusMax * sin };
            var p3 = new Point { X = centerX + radiusMin * cos, Y = centerY - radiusMin * sin };

            var intRound = new Size(radiusMin, radiusMin);
            var extRound = new Size(radiusMax * 1.0, radiusMax * 1.0);

            var isLargeArc = Math.Abs(angleFrom - angleTo) >= Math.PI;

            if (path == null)
            {
                var firstPoint = p0;
                var segments = new PathSegment[]
                {
                    new PolyLineSegment(new[] { p1 }, true),
                    new ArcSegment(p2, extRound, 0, isLargeArc, SweepDirection.Clockwise, true),
                    new PolyLineSegment(new[] { p3 }, true),
                    new ArcSegment(p0, intRound, 0, isLargeArc, SweepDirection.Counterclockwise, true)
                };

                path = new Path { Data = new PathGeometry { Figures = new PathFigureCollection { new PathFigure(firstPoint, segments, false) } } };

                AddElement(path);
            }
            else
            {
                var figure = (path.Data as PathGeometry).Figures[0];
                figure.StartPoint = p0;
                (figure.Segments[0] as PolyLineSegment).Points[0] = p1;
                (figure.Segments[1] as ArcSegment).Point = p2;
                (figure.Segments[1] as ArcSegment).Size = extRound;
                (figure.Segments[1] as ArcSegment).IsLargeArc = isLargeArc;
                (figure.Segments[2] as PolyLineSegment).Points[0] = p3;
                (figure.Segments[3] as ArcSegment).Point = p0;
                (figure.Segments[3] as ArcSegment).Size = intRound;
                (figure.Segments[3] as ArcSegment).IsLargeArc = isLargeArc;
            }

            path.StrokeThickness = source.BorderThickness;
            path.Stroke = new SolidColorBrush(source.BorderColor);
            path.Fill = new SolidColorBrush(source.BackgroundColor);
        }
    }
}
