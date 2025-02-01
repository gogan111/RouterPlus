using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RouterPlus.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Converts boolean value to Visibility (Visible or Collapsed)
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Converts Visibility back to boolean (true for Visible, false for Collapsed)
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}