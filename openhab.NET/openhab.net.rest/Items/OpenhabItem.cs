using openhab.net.rest.Core;
using openhab.net.rest.DataSource;
using openhab.net.rest.Json;
using System;
using System.ComponentModel;
using System.Linq;

namespace openhab.net.rest.Items
{
    public abstract class OpenhabItem : IOpenhabElement, IElementObservable
    {
        IElementObserver _observer;

        public event PropertyChangedEventHandler PropertyChanged;
        

        internal OpenhabItem(ItemObject original, IElementObserver observer)
        {
            Name = original.Name;
            Value = original.State;
            Type = original.ItemType;
            Link = new Uri(original.Link);
            (_observer = observer)?.Subscribe(this);
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
        

        protected void Update(string value)
        {
            Value = value;
            _observer?.Notify(this);
            FireValueChanged();
        }

        protected void FireValueChanged()
        {
            var args = new PropertyChangedEventArgs(nameof(Value));
            PropertyChanged?.Invoke(this, args);
        }

        public void OnNotify(IOpenhabElement element)
        {
            var other = element as OpenhabItem;
            if (other?.Name == Name) {
                Value = other.Value;
                FireValueChanged();
            }
        }

        public bool IsEqual(IOpenhabElement element)
        {
            var other = element as OpenhabItem;
            if (other?.Name == Name) {
                return Value == other.Value;
            }
            return true;
        }
    }
}
