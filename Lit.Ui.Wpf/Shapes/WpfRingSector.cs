using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// WPF ring sector shape.
    /// </summary>
    public class WpfRingSector : WpfShapeBase
    {
        /// <summary>
        /// Generated path.
        /// </summary>
        public Path Path => path;

        private Path path;

        /// <summary>
        /// Reference X coordinate.
        /// </summary>
        public double CenterX { get => centerX; set => SetProp(ref centerX, value, Change.Layout, nameof(CenterX)); }

        private double centerX;

        /// <summary>
        /// Reference Y coordinate.
        /// </summary>
        public double CenterY { get => centerY; set => SetProp(ref centerY, value, Change.Layout, nameof(CenterY)); }

        private double centerY;

        /// <summary>
        /// Minimum radius.
        /// </summary>
        public double RadiusMin { get => radiusMin; set => SetProp(ref radiusMin, value, Change.Layout, nameof(RadiusMin)); }

        private double radiusMin;

        /// <summary>
        /// Maximum radius.
        /// </summary>
        public double RadiusMax { get => radiusMax; set => SetProp(ref radiusMax, value, Change.Layout, nameof(RadiusMax)); }

        private double radiusMax;

        /// <summary>
        /// Starting angle.
        /// </summary>
        public double AngleFrom { get => angleFrom; set => SetProp(ref angleFrom, value, Change.Layout, nameof(AngleFrom)); }

        private double angleFrom;

        /// <summary>
        /// Ending angle.
        /// </summary>
        public double AngleTo { get => angleTo; set => SetProp(ref angleTo, value, Change.Layout, nameof(AngleTo)); }

        private double angleTo;

        /// <summary>
        /// Border thickness.
        /// </summary>
        public double? BorderThickness { get => borderThickness; set => SetProp(ref borderThickness, value, Change.Aspect, nameof(BorderThickness)); }

        private double? borderThickness;

        public Color? BorderColor { get => borderColor; set => SetProp(ref borderColor, value, Change.Aspect, nameof(BorderColor)); }

        private Color? borderColor;

        public Color? BackgroundColor { get => backgroundColor; set => SetProp(ref backgroundColor, value, Change.Aspect, nameof(BackgroundColor)); }

        private Color? backgroundColor;

        protected override void UpdateItems()
        {
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

            path.StrokeThickness = borderThickness ?? 1;
            path.Stroke = borderColor.HasValue ? new SolidColorBrush(borderColor.Value) : null;
            path.Fill = backgroundColor.HasValue ? new SolidColorBrush(backgroundColor.Value) : null;
        }
    }
}
