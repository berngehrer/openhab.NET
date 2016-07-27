using openhab.net.rest.Items;
using Windows.UI;
using System;
using Windows.UI.Xaml.Data;

namespace App1.Converter
{
    class ColorItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rgb = value as ColorRGB;
            if (rgb != null) {
                return new Color { R = (byte)rgb.R, G = (byte)rgb.G, B = (byte)rgb.B };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Color) {
                var color = (Color)value;
                return new ColorRGB { R = color.R, G = color.G, B = color.B };
            }
            return new ColorRGB { R = 0, G = 0, B = 0 };
        }
    }
}
