using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class SwitchItem : OpenhabItem
    {
        internal SwitchItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }


        void TurnOn() { }
        void TurnOff() { }
        void Toogle() { }
    }
}
