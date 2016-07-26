using openhab.net.rest.Items;
using System;
using Windows.UI.Xaml.Data;

namespace App1
{
    class ItemValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = value as OpenhabItem;
            if (item != null)
                switch (item.Type)
                {
                    case ItemType.Switch:
                        return ((SwitchItem)value).Value;
                }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
