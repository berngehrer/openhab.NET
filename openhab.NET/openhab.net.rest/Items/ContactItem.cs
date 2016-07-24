using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class ContactItem : OpenhabItem
    {
        const string OpenState = "open";
        const string ClosedState = "close";

        internal ContactItem(ItemObject original) : base(original)
        {
            if (IsInitialized) {
                _value = StringToValue();
            }
        }


        bool _value;
        //public new bool Value
        //{
        //    get { return _value; }
        //    set { _value = value; ShadowUpdate(ValueToString()); }
        //}


        bool StringToValue() => base.Value.Equals(OpenState);

        //string ValueToString() => (Value) ? OpenState : ClosedState;
    }
}
