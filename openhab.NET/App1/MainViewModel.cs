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
        }

        ItemWrapper<SwitchItem> _led;
        public ItemWrapper<SwitchItem> TvLed
        {
            get { return _led; }
            set { Set(ref _led, value); }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
