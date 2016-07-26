using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class NumberItem : OpenhabItem
    {
        internal NumberItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
            Syncronize();
        }

        float _value;
        public new float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Update(string.Format("{0:#.00}", value));
            }
        }

        protected override void Syncronize()
        {
            if (IsInitialized) {
                ValueParser.TryParse(base.Value, out _value);
            }
        }
    }
}
