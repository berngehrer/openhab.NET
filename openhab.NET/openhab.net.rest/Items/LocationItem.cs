using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class LocationItem : OpenhabItem
    {
        internal LocationItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }


        float _latitude;
        public float Latitude
        {
            get { return _latitude; }
            set { _latitude = value; FromNative(ToNative()); }
        }

        float _longitude;
        public float Longitude
        {
            get { return _longitude; }
            set { _longitude = value; FromNative(ToNative()); }
        }


        protected override void Syncronize()
        {
            if (IsInitialized) {
                var point = Value.Split(',');
                if (point.Length >= 2)  {
                    ValueParser.TryParse(point[0], out _latitude);
                    ValueParser.TryParse(point[1], out _longitude);
                }
            }
        }

        public override void FromNative(object obj)
        {
            var point = obj as LocationPoint;
            if (point != null)
            {
                _latitude = point.Latitude;
                _longitude = point.Longitude;
                Update($"{ToFormat(_latitude)},{ToFormat(_longitude)}");
            }
        }

        string ToFormat(float f) => f.ToString().Replace(',', '.');

        public override object ToNative()
        {
            return new LocationPoint { Latitude = this.Latitude, Longitude = this.Longitude };
        }
    }

    public class LocationPoint
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
