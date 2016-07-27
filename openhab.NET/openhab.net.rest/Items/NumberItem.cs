using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class NumberItem : OpenhabItem
    {
        internal NumberItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        float _value;
        public new float Value
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
            if (obj is float) {
                Update(string.Format("{0:#.00}", obj));
            }
        }

        public override object ToNative() => _value;
    }
}
