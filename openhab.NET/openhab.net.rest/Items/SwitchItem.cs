using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Items
{
    public class SwitchItem : OpenhabItem
    {
        const string OnState = "ON";
        const string OffState = "OFF";
        
        internal SwitchItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        bool _value;
        public new bool Value
        {
            get { return _value; }
            set { _value = value; FromNative(value); }
        }
        
        public void Toggle()
        {
            Value = !Value;
        }


        protected override void Syncronize()
        {
            if (IsInitialized) {
                _value = base.Value.Equals(OnState, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public override void FromNative(object obj)
        {
            if (obj is bool) {
                Update((bool)obj ? OnState : OffState);
            }
        }

        public override object ToNative() => _value;
    }
}
