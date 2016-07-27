using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Items
{
    public class ContactItem : OpenhabItem
    {
        const string OpenState = "OPEN";
        const string ClosedState = "CLOSE";

        internal ContactItem(ItemObject original, IElementObserver observer) : base(original, observer)
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
                _value = base.Value.Equals(OpenState, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public override void FromNative(object obj)
        {
            if (obj is bool)
            {
                Update((bool)obj ? OpenState : ClosedState);
            }
        }

        public override object ToNative() => _value;
    }
}
