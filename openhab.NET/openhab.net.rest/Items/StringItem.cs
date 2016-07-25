using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class StringItem : OpenhabItem
    {
        internal StringItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        public new string Value
        {
            get { return base.Value; }
            set { Update(value); }
        }
    }
}
