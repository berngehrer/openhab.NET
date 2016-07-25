using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Items
{
    public class DateTimeItem : OpenhabItem
    {
        internal DateTimeItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
            if (IsInitialized) {
                ValueParser.TryParse(base.Value, out _value);
            }
        }

        DateTime _value;
        public new DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Update(value.ToString());
            }
        }
    }
}
