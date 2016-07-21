using openhab.net.rest.Json;
using System;
using System.Linq;

namespace openhab.net.rest.Items
{
    public abstract class OpenhabItem : IOpenhabElement, IEquatable<OpenhabItem>
    {
        string[] _notInitialized = { "", "Undefined", "Uninitialized", };

        internal OpenhabItem(ItemObject original)
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

        public static bool operator ==(OpenhabItem a, OpenhabItem b)
        {
            return a.Name.Equals(b.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool operator !=(OpenhabItem a, OpenhabItem b)
        {
            return !a.Name.Equals(b.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool Equals(OpenhabItem other) => this == other;

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
