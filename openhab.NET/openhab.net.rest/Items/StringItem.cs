using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class StringItem : OpenhabItem
    {
        internal StringItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }
        
        public override void FromNative(object obj)
        {
            if (obj is string) {
                Update(obj?.ToString());
            }
        }

        public override object ToNative() => Value;

        protected override void Syncronize()
        {
        }
    }
}
