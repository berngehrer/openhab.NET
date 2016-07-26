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
            _value = IsInitialized && base.Value.Equals(OnState);
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

        public void TurnOn() { }
        public void TurnOff() { }
        public void Toggle() { }
    }
}
