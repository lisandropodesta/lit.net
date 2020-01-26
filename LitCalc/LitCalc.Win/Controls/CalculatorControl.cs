using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.CircularMenu;

namespace LitCalc.Win.Controls
{
    /// <summary>
    /// Calculator control.
    /// </summary>
    public class CalculatorControl : Canvas, IWpfCircularMenuContext
    {
        private const int MarginPx = 10;

        private const int RoundPx = 10;

        static CalculatorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalculatorControl), new FrameworkPropertyMetadata(typeof(CalculatorControl)));
        }

        private Path display;

        CircularMenu<WpfCircularMenuItem> ICircularMenuContext<WpfCircularMenuItem>.ContextMenu => contextMenu;

        private readonly WpfCircularMenu contextMenu;

        public CalculatorControl()
        {
            SizeChanged += MainControl_SizeChanged;
            MouseDown += MainControl_MouseDown;
            Background = new SolidColorBrush(Colors.White);

            var hg = RenderSize.Height;
            var wd = RenderSize.Width;

            UpdateControls();
            Children.Add(display);

            // Context menu creation
            contextMenu = new WpfCircularMenu
            {
                CenterRadius = 20,
                RingSize = 50,
                Items = new List<WpfCircularMenuItem>()
                {
                    new WpfCircularMenuItem
                    {
                        Text = "Left",
                        TargetAngle = Math.PI,
                        TargetSize = Math.PI / 6,
                        ShapeBackgroundColor = Colors.Yellow,
                        Command = Option1
                    },
                    new WpfCircularMenuItem
                    {
                        Text = "2",
                        ShapeBackgroundColor = Colors.Red,
                        Command = Option1
                    },
                    new WpfCircularMenuItem
                    {
                        Text = "2",
                        ShapeBackgroundColor = Colors.Green,
                        Command = Option1
                    },
                    new WpfCircularMenuItem
                    {
                        Text = "1",
                        ShapeBackgroundColor = Colors.Blue,
                        Command = Option1
                    },
                    new WpfCircularMenuItem
                    {
                        Text = "Top",
                        TargetAngle = Math.PI/2,
                        TargetSize = Math.PI / 6,
                        ShapeBackgroundColor = Colors.Yellow,
                        Command = Option1
                    },
                    new WpfCircularMenuItem
                    {
                        Text = "Top",
                        TargetAngle = 0,
                        TargetSize = Math.PI / 6,
                        ShapeBackgroundColor = Colors.Yellow,
                        Command = Option1
                    },
                }
            };
        }

        private void Option1(WpfCircularMenuItem item, object sender, object parameter)
        {

        }

        private void MainControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
        }

        private void MainControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            var hg = Math.Max(RenderSize.Height, 100);
            var wd = Math.Max(RenderSize.Width, 100);

            var xc = new double[]
            {
                MarginPx,
                MarginPx + RoundPx,
                wd - MarginPx - RoundPx,
                wd - MarginPx
            };

            var yc = new double[]
            {
                MarginPx,
                MarginPx + RoundPx,
                hg - MarginPx - RoundPx,
                hg - MarginPx
            };

            var points = new Point[]
            {
                new Point(xc[0], yc[1]),
                new Point(xc[0], yc[2]),
                new Point(xc[1], yc[3]),
                new Point(xc[2], yc[3]),
                new Point(xc[3], yc[2]),
                new Point(xc[3], yc[1]),
                new Point(xc[2], yc[0]),
                new Point(xc[1], yc[0])
            };

            if (display == null)
            {
                var roundSize = new Size(RoundPx, RoundPx);

                var firstPoint = points[0];
                var segments = new PathSegment[]
                {
                    new PolyLineSegment(new[] { points[1] }, true),
                    new ArcSegment(points[2], roundSize, 0, false, SweepDirection.Counterclockwise, true),
                    new PolyLineSegment(new[] { points[3] }, true),
                    new ArcSegment(points[4], roundSize, 0, false, SweepDirection.Counterclockwise, true),
                    new PolyLineSegment(new[] { points[5] }, true),
                    new ArcSegment(points[6], roundSize, 0, false, SweepDirection.Counterclockwise, true),
                    new PolyLineSegment(new[] { points[7] }, true),
                    new ArcSegment(points[0], roundSize, 0, false, SweepDirection.Counterclockwise, true),
                };

                display = new Path
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Colors.LightYellow),
                    Data = new PathGeometry
                    {
                        Figures = new PathFigureCollection { new PathFigure(firstPoint, segments, false) }
                    }
                };
            }
            else
            {
                var figure = (display.Data as PathGeometry).Figures[0];
                figure.StartPoint = points[0];

                var pointIndex = 1;
                foreach (var segment in figure.Segments)
                {
                    if (segment is PolyLineSegment ps)
                    {
                        ps.Points[0] = points[pointIndex];
                    }
                    else if (segment is ArcSegment arc)
                    {
                        arc.Point = points[pointIndex];
                    }

                    pointIndex = (pointIndex + 1) % points.Length;
                }
            }
        }

        //private Storyboard ellipseStoryboard;

        //private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    StopStoryboard(ref ellipseStoryboard);

        //    ellipseStoryboard = new Storyboard();
        //    ellipseStoryboard.Completed += (s, ev) => { ellipse.Fill = new SolidColorBrush(Colors.Yellow); StopStoryboard(ref ellipseStoryboard); };

        //    var animation = new BrushAnimation
        //    {
        //        To = new SolidColorBrush(Colors.Yellow),
        //        Duration = new Duration(TimeSpan.FromMilliseconds(2000))
        //    };

        //    ellipseStoryboard.Children.Add(animation);
        //    Storyboard.SetTarget(animation, ellipse);
        //    Storyboard.SetTargetProperty(animation, new PropertyPath(Shape.FillProperty));
        //    ellipseStoryboard.Begin();
        //    //ellipse.Fill = new SolidColorBrush(Colors.Yellow);
        //}

        //private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    StopStoryboard(ref ellipseStoryboard);

        //    ellipseStoryboard = new Storyboard();
        //    ellipseStoryboard.Completed += (s, ev) => { ellipse.Fill = new SolidColorBrush(Colors.Cyan); StopStoryboard(ref ellipseStoryboard); };

        //    var animation = new BrushAnimation
        //    {
        //        To = new SolidColorBrush(Colors.Cyan),
        //        Duration = new Duration(TimeSpan.FromMilliseconds(2000))
        //    };

        //    ellipseStoryboard.Children.Add(animation);
        //    Storyboard.SetTarget(animation, ellipse);
        //    Storyboard.SetTargetProperty(animation, new PropertyPath(Shape.FillProperty));
        //    ellipseStoryboard.Begin();
        //    //ellipse.Fill = new SolidColorBrush(Colors.Cyan);
        //}

        //private void StopStoryboard(ref Storyboard storyboard)
        //{
        //    if (storyboard != null)
        //    {
        //        storyboard.Stop();
        //        storyboard.Children.Clear();
        //        storyboard = null;
        //    }
        //}
    }
}
