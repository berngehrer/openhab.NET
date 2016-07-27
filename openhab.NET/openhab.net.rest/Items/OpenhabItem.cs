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
            _value = original?.State;
            Type = original.ItemType;
            Link = new Uri(original.Link);
            (_observer = observer)?.Subscribe(this);
            Syncronize();
        }

        public Uri Link { get; }
        public string Name { get; }
        public ItemType Type { get; }
        public string Value => _value;

        public bool IsInitialized
        {
            get { return Value != null && !(new[]{ "", "Undefined", "Uninitialized" }).Contains(Value); }
        }
        
        public override string ToString() => Value;
        

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

        protected void FireValueChanged()
        {
            Syncronize();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update from Observer
        /// </summary>
        public void OnNotify(IOpenhabElement element)
        {
            var other = element as OpenhabItem;
            if (!IsEqual(other))
            {
                _value = other.Value;
                FireValueChanged();
            }
        }

        /// <summary>
        /// Update from inheritance object
        /// </summary>
        protected void Update(string value)
        {
            if (Value != value)
            {
                _value = value;
                FireValueChanged();
                _observer?.Notify(this);
            }
        }        

        protected abstract void Syncronize();
        public abstract void FromNative(object obj);
        public abstract object ToNative();
    }
}
