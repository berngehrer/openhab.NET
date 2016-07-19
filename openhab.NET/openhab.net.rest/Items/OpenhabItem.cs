using openhab.net.rest.JsonEntities;
using System;
using System.Linq;

namespace openhab.net.rest.Items
{
    public abstract class OpenhabItem : IEquatable<OpenhabItem>
    {
        string[] _notInitialized = { "", "Undefined", "Uninitialized", };

        protected OpenhabItem(ItemObject original)
        {
            Name = original.Name;
            Value = original.State;
            Type = original.ItemType;
            Link = new Uri(original.Link);
        }

        public Uri Link { get; }
        public string Name { get; }
        public ItemType Type { get; }
        public string Value { get; protected set; }

        public bool IsInitialized => !_notInitialized.Contains(Value);
        

        protected bool TryParseValue<T>(out T obj)
        {
            obj = default(T);

            if (typeof(T).Is<int>())
            {
                int i;
                if (int.TryParse(Value, out i)) {
                    obj = (T)(object)i;
                    return true;
                }
            }
            if (typeof(T).Is<float>())
            {
                float f;
                if (float.TryParse(Value, out f)) {
                    obj = (T)(object)f;
                    return true;
                }
            }
            if (typeof(T).Is<DateTime>())
            {
                DateTime dt;
                if (DateTime.TryParse(Value, out dt)) {
                    obj = (T)(object)dt;
                    return true;
                }
            }
            return false;
        }


        public static bool operator ==(OpenhabItem a, OpenhabItem b) => a.Name == b.Name;

        public static bool operator !=(OpenhabItem a, OpenhabItem b) => a.Name != b.Name;
        
        public bool Equals(OpenhabItem other) => Name == other?.Name;

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            var other = obj as OpenhabItem;
            if (other != null)
                return Equals(other);
            return base.Equals(obj);
        }
    }
}
