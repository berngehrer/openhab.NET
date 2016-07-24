using openhab.net.rest.Json;
using System;
using System.ComponentModel;
using System.Linq;

namespace openhab.net.rest.Items
{
    public abstract class OpenhabItem : IOpenhabElement
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
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
        public string Value { get; private set; }

        public bool IsInitialized
        {
            get { return !(new[]{ "", "Undefined", "Uninitialized" }).Contains(Value); }
        }
        
        public override string ToString() => Value;
        

        public bool ShadowUpdate(IOpenhabElement element)
        {
            var value = ((OpenhabItem)element).Value;

            bool hasChanged = !Value.Equals(value);
            if (hasChanged) {
                Value = value;
            }
            return hasChanged;
        }

        bool ShadowUpdate(string value)
        {
            bool hasChanged = !Value.Equals(value);
            if (hasChanged) {
                Value = value;
            }
            return hasChanged;
        }

        protected void FireValueChanged()
        {
            var args = new PropertyChangedEventArgs(nameof(Value));
            PropertyChanged?.Invoke(this, args);
        }
    }
}
