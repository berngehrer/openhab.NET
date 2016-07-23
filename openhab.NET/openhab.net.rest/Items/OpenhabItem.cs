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
        public bool HasChanged { get; private set; }

        public bool IsInitialized
        {
            get { return !(new[]{ "", "Undefined", "Uninitialized" }).Contains(Value); }
        }
        
        public override string ToString() => Value;

        public void Reset()
        {
            HasChanged = false;
        }

        protected void UpdateValue(string value)
        {
            if (!Value.Equals(value)) {
                Value = value;
                HasChanged = true;
                FirePropertyChanged();
            }
        }

        void FirePropertyChanged()
        {
            var args = new PropertyChangedEventArgs(nameof(Value));
            PropertyChanged?.Invoke(this, args);
        }
    }
}
