using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;
using System.Linq;

namespace openhab.net.rest.Items
{
    public abstract class OpenhabItem : IOpenhabElement, IElementObservable
    {
        string _value;
        IElementObserver _observer;

        public event EventHandler Changed;
        

        internal OpenhabItem(ItemObject original, IElementObserver observer)
        {
            Name = original.Name;
            _value = original.State;
            Type = original.ItemType;
            Link = new Uri(original.Link);
            (_observer = observer)?.Subscribe(this);
        }

        public Uri Link { get; }
        public string Name { get; }
        public ItemType Type { get; }
        public string Value => _value;

        public bool IsInitialized
        {
            get { return !(new[]{ "", "Undefined", "Uninitialized" }).Contains(Value); }
        }
        
        public override string ToString() => Value;
        

        public void OnNotify(IOpenhabElement element)
        {
            var other = element as OpenhabItem;
            if (!IsEqual(other)) {
                _value = other.Value;
                Syncronize();
                FireValueChanged();
            }
        }

        public bool IsEqual(IOpenhabElement element)
        {
            var other = element as OpenhabItem;
            if (Equals(other)) {
                return Value == other.Value;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            var other = obj as OpenhabItem;
            return other?.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }


        public void FromNative(object obj)
        {
            Update((bool)obj ? "ON" : "OFF");
            Syncronize();
        }

        public object ToNative()
        {
            return IsInitialized && Value.Equals("ON");
        }

        protected void Update(string value)
        {
            if (Value != value)
            {
                _value = value;
                FireValueChanged();
                _observer?.Notify(this);
            }
        }

        protected void FireValueChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Syncronize() { }
    }
}
