using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Lit.Ui.Wpf.Animation
{
    /// <summary>
    /// Brush animation.
    /// </summary>
    public class BrushAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof(BrushAnimation.From), typeof(Brush), typeof(BrushAnimation));

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof(BrushAnimation.To), typeof(Brush), typeof(BrushAnimation));

        /// <summary>
        /// From brush.
        /// </summary>
        public Brush From
        {
            get { return (Brush)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        /// <summary>
        /// To brush.
        /// </summary>
        public Brush To
        {
            get { return (Brush)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public override Type TargetPropertyType { get { return typeof(Brush); } }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (!animationClock.CurrentProgress.HasValue)
            {
                return Brushes.Transparent;
            }

            var srcBrush = this.From ?? defaultOriginValue as Brush;

            if (animationClock.CurrentProgress.Value == 0)
            {
                return srcBrush;
            }

            var dstBrush = this.To ?? defaultDestinationValue as Brush;

            if (animationClock.CurrentProgress.Value == 1)
            {
                return dstBrush;
            }

            return new VisualBrush(new Border()
            {
                Width = 1,
                Height = 1,
                Background = srcBrush,
                Child = new Border()
                {
                    Background = dstBrush,
                    Opacity = animationClock.CurrentProgress.Value
                }
            });
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BrushAnimation();
        }
    }
}
