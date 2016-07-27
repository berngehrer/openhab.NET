using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class RollershutterItem : OpenhabItem
    {
        internal RollershutterItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        int _value;
        public new int Value
        {
            get { return _value; }
            set { _value = value; FromNative(value); }
        }

        void Open() => FromNative(0);

        void Close() => FromNative(100);


        protected override void Syncronize()
        {
            if (IsInitialized) {
                ValueParser.TryParse(base.Value, out _value);
            }
        }

        public override void FromNative(object obj)
        {
            if (obj is int) {
                Update(obj.ToString());
            }
        }

        public override object ToNative() => Value;
    }
}
