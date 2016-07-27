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
            var settings = new OpenhabSettings("192.168.178.69");
            var strategy = new UpdateStrategy(TimeSpan.FromSeconds(3));

            _context = new ItemContext(settings, strategy);

            LoadItem();
        }

        async void LoadItem()
        {
            TvLed = await _context.GetByName<SwitchItem>("MQTT_TVLED_POW");
            TvLedColor = await _context.GetByName<ColorItem>("MQTT_TVLED_COLOR");
            LastUpdate = await _context.GetByName<DateTimeItem>("Astro_Moon_New");
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

        ItemWrapper<DateTimeItem> _updateDate;
        public ItemWrapper<DateTimeItem> LastUpdate
        {
            get { return _updateDate; }
            set { Set(ref _updateDate, value); }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
