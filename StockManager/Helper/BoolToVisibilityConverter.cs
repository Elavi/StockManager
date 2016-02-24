using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace StockManager.Helper
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool) value;
            if (boolValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility) value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
