using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Items
{
    public class DateTimeItem : OpenhabItem
    {
        internal DateTimeItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        DateTime _value;
        public new DateTime Value
        {
            get { return _value; }
            set { _value = value; FromNative(value); }
        }


        protected override void Syncronize()
        {
            if (IsInitialized) {
                ValueParser.TryParse(base.Value, out _value);
            }
        }

        public override void FromNative(object obj)
        {
            if (obj is DateTime) {
                Update(obj.ToString());
            }
        }

        public override object ToNative() => _value;
    }
}
