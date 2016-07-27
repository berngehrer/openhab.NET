using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class GroupItem : OpenhabItem
    {
        internal GroupItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }


        protected override void Syncronize()
        {
        }

        public override void FromNative(object obj)
        {
        }

        public override object ToNative() => Value;
    }
}
