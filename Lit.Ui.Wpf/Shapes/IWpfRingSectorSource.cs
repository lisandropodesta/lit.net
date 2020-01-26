using System.Windows.Media;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// Drawing parameters for a WPF ring sector.
    /// </summary>
    public interface IWpfRingSectorSource : IWpfShapeSource
    {
        /// <summary>
        /// Reference X coordinate.
        /// </summary>
        double CenterX { get; }

        /// <summary>
        /// Reference Y coordinate.
        /// </summary>
        double CenterY { get; }

        /// <summary>
        /// Minimum radius.
        /// </summary>
        double RadiusMin { get; }

        /// <summary>
        /// Maximum radius.
        /// </summary>
        double RadiusMax { get; }

        /// <summary>
        /// Starting angle.
        /// </summary>
        double AngleFrom { get; }

        /// <summary>
        /// Ending angle.
        /// </summary>
        double AngleTo { get; }

        /// <summary>
        /// Border thickness.
        /// </summary>
        double BorderThickness { get; }

        /// <summary>
        /// Shape border color.
        /// </summary>
        Color BorderColor { get; }

        /// <summary>
        /// Shaper background color.
        /// </summary>
        Color BackgroundColor { get; }
    }
}
