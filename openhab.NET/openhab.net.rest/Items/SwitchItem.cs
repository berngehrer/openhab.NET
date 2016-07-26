using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class SwitchItem : OpenhabItem
    {
        const string OnState = "ON";
        const string OffState = "OFF";
        
        internal SwitchItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
            Syncronize();
        }

        bool _value;
        public new bool Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Update(value ? OnState : OffState);
            }
        }

        protected override void Syncronize()
        {
            _value = IsInitialized && base.Value.Equals(OnState);
        }

        public void Toggle()
        {
            Value = !Value;
        }
    }
}
