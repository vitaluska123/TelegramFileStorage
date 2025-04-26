using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace TelegramFileStorage.Views
{
    public class StringToBitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string uri && !string.IsNullOrWhiteSpace(uri))
            {
                var assets = AssetLoader.Exists(new Uri(uri))
                    ? AssetLoader.Open(new Uri(uri))
                    : null;
                if (assets != null)
                {
                    return new Bitmap(assets);
                }
            }
            return null;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
