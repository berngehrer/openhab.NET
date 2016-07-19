using openhab.net.rest.JsonEntities;

namespace openhab.net.rest.Items
{
    public class StringItem : OpenhabItem
    {
        internal StringItem(ItemObject original) : base(original)
        {
        }

        public new string Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}
