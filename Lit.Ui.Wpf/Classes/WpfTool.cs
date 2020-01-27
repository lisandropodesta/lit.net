namespace Lit.Ui.Wpf
{
    public static class WpfTool
    {
        /// <summary>
        /// Converts System.Drawing.Color into System.Windows.Media.Color.
        /// </summary>
        public static System.Windows.Media.Color WpfColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts System.Windows.Media.Color into System.Drawing.Color.
        /// </summary>
        public static System.Drawing.Color WinColor(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
