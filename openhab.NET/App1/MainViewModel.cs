using GalaSoft.MvvmLight;
using openhab.net.rest;
using openhab.net.rest.Items;
using System;
using System.Threading.Tasks;

namespace App1
{
    class MainViewModel : ViewModelBase, IDisposable
    {
        ItemContext _context = new ItemContext("homepi");

        public MainViewModel()
        {
            LoadItem().ConfigureAwait(false);
        }

        async Task LoadItem()
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
            set { Set(ref _ledColor, value); R = value.Item.Red; G = value.Item.Green; B = value.Item.Blue; V = value.Item.Brightness; }
        }

        double _r;
        public double R
        {
            get { return _r; }
            set { Set(ref _r, value); TvLedColor.Item.Red = (int)value; }
        }

        double _g;
        public double G
        {
            get { return _g; }
            set { Set(ref _g, value); TvLedColor.Item.Green = (int)value; }
        }

        double _b;
        public double B
        {
            get { return _b; }
            set { Set(ref _b, value); TvLedColor.Item.Blue = (int)value; }
        }
        
        double _v;
        public double V
        {
            get { return _v; }
            set { Set(ref _v, value); TvLedColor.Item.Brightness = (float)value; }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
