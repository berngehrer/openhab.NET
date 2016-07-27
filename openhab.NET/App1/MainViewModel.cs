using GalaSoft.MvvmLight;
using openhab.net.rest;
using openhab.net.rest.Items;
using System;

namespace App1
{
    class MainViewModel : ViewModelBase, IDisposable
    {
        ItemContext _context;

        public MainViewModel()
        {
            _context = new ItemContext("homepi"); 
            LoadItem();
        }

        async void LoadItem()
        {
            TvLed = await _context.GetByName<SwitchItem>("MQTT_TVLED_POW");
            TvLedColor = await _context.GetByName<ColorItem>("MQTT_TVLED_COLOR");
        }

        ItemWrapper<SwitchItem> _led;
        public ItemWrapper<SwitchItem> TvLed
        {
            get { return _led; }
            set { Set(ref _led, value); }
        }

        ItemWrapper<ColorItem> _ledColor;
        public ItemWrapper<ColorItem> TvLedColor
        {
            get { return _ledColor; }
            set { Set(ref _ledColor, value); }
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
