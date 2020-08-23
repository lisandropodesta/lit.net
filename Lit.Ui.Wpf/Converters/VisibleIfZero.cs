using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Lit.DataType;

namespace Lit.Ui.Wpf.Converters
{
    /// <summary>
    /// Visible if zero converter.
    /// </summary>
    public class VisibleIfZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !TypeHelper.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
