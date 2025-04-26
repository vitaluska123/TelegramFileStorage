using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TelegramFileStorage.Views
{
    public class TypeIsNotExplanationConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null || !string.Equals(value.ToString(), "explanation", StringComparison.OrdinalIgnoreCase);
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
