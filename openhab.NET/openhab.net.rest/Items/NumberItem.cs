using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class NumberItem : OpenhabItem
    {
        internal NumberItem(ItemObject original) : base(original)
        {
            if (IsInitialized) {
                ValueParser.TryParse(base.Value, out _value);
            }
        }

        float _value;
        public new float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                UpdateValue(string.Format("{0:#.00}", value));
            }
        }
    }
}
