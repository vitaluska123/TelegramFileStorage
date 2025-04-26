using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TelegramFileStorage.Views
{
    public class IntGreaterThanZeroConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int i)
                return i > 0;
            return false;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
