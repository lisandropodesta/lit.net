using System.Windows.Controls;

namespace Lit.Ui.Wpf.Shapes
{
    /// <summary>
    /// Drawing parameters for a WPF shape.
    /// </summary>
    public interface IWpfShapeSource
    {
        /// <summary>
        /// Visibility control property.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Canvas control property.
        /// </summary>
        Canvas Canvas { get; }
    }
}
