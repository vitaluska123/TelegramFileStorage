using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TelegramFileStorage.Views
{
    public class BoolToThemeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? "Тёмная" : "Светлая";
            return "Светлая";
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string s)
                return s == "Тёмная";
            return false;
        }
    }
}
