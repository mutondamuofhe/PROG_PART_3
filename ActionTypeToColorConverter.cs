using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PROG_PART_3.Models;

namespace PROG_PART_3
{
    public class ActionTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string actionType)
            {
                switch (actionType)
                {
                    case ActivityLog.ActionTypes.TaskAdded:
                        return new SolidColorBrush(Color.FromRgb(46, 204, 113)); // Green

                    case ActivityLog.ActionTypes.TaskCompleted:
                        return new SolidColorBrush(Color.FromRgb(52, 152, 219)); // Blue

                    case ActivityLog.ActionTypes.TaskRemoved:
                        return new SolidColorBrush(Color.FromRgb(231, 76, 60)); // Red

                    case ActivityLog.ActionTypes.ReminderSet:
                        return new SolidColorBrush(Color.FromRgb(230, 126, 34)); // Orange

                    case ActivityLog.ActionTypes.QuizStarted:
                    case ActivityLog.ActionTypes.QuizCompleted:
                        return new SolidColorBrush(Color.FromRgb(142, 68, 173)); // Purple

                    case ActivityLog.ActionTypes.ChatInteraction:
                        return new SolidColorBrush(Color.FromRgb(52, 73, 94)); // Dark Blue

                    default:
                        return new SolidColorBrush(Color.FromRgb(149, 165, 166)); // Gray
                }
            }

            return new SolidColorBrush(Color.FromRgb(149, 165, 166)); // Default gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
