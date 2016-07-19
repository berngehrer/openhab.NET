using openhab.net.rest.JsonEntities;

namespace openhab.net.rest.Items
{
    public class NumberItem : OpenhabItem
    {
        internal NumberItem(ItemObject original) : base(original)
        {
            if (IsInitialized) {
                TryParseValue(out _value);
            }
        }

        private float _value;
        public new float Value
        {
            get { return _value; }
            set { base.Value = string.Format("{0:#.00}", value); }
        }
    }
}
