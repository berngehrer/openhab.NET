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
            var item = await _context.GetByName<SwitchItem>("MQTT_TVLED_POW");
            TvLed = new ItemWrapper<SwitchItem, bool>(item);
            //var item2 = await _context.GetByName<NumberItem>("Weather_Temperature");
            //Temperature = new ItemWrapper<OpenhabItem, string>(item2);
        }

        ItemWrapper<SwitchItem, bool> _led;
        public ItemWrapper<SwitchItem, bool> TvLed
        {
            get { return _led; }
            set { Set(ref _led, value); }
        }

        ItemWrapper<OpenhabItem, string> _temp;
        public ItemWrapper<OpenhabItem, string> Temperature
        {
            get { return _temp; }
            set { Set(ref _temp, value); }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
