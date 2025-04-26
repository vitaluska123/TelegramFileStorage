using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia.Media;
using TelegramFileStorage.ViewModels;

namespace TelegramFileStorage.Views
{
    public class NotificationTypeToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is NotificationType type)
            {
                return type switch
                {
                    NotificationType.Error => Brushes.Red,
                    NotificationType.Success => Brushes.LimeGreen,
                    NotificationType.Alert => Brushes.Gold,
                    NotificationType.Loading => Brushes.DodgerBlue,
                    NotificationType.Accept => Brushes.White,
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NotificationTypeIsLoadingConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is NotificationType type && type == NotificationType.Loading;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NotificationTypeIsAcceptConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is NotificationType type && type == NotificationType.Accept;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
