using System;
using Avalonia.Data.Converters;

namespace TelegramFileStorage.Views
{
    public class NullToBoolConverter : IValueConverter
    {
        public static readonly NullToBoolConverter Instance = new NullToBoolConverter();
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && !string.IsNullOrEmpty(value.ToString());
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
