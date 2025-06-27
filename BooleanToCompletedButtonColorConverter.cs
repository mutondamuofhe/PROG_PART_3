using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PROG_PART_3
{
    public class BooleanToCompletedButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted
                    ? new SolidColorBrush(Color.FromRgb(46, 204, 113)) // Green for completed tasks
                    : new SolidColorBrush(Color.FromRgb(52, 152, 219)); // Blue for pending tasks
            }

            return new SolidColorBrush(Color.FromRgb(52, 152, 219)); // Default blue
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}