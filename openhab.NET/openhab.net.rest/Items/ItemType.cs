using openhab.net.rest.ComponentModel;

namespace openhab.net.rest.Items
{
    /// <summary>
    /// Possible results of item types from request
    /// </summary>
    public enum ItemType
    {
        Unknown,

        [FieldValue("GroupItem")]
        Group,
        [FieldValue("StringItem")]
        String,
        [FieldValue("SwitchItem")]
        Switch,
        [FieldValue("DimmerItem")]
        Dimmer,
        [FieldValue("NumberItem")]
        Number,
        [FieldValue("ColorItem")]
        Color,
        [FieldValue("DateTimeItem")]
        DateTime,
        [FieldValue("CallItem")]
        Call,
        [FieldValue("ContactItem")]
        Contact,
        [FieldValue("LocationItem")]
        Location,
        [FieldValue("RollershutterItem")]
        Rollershutter
    }
}
