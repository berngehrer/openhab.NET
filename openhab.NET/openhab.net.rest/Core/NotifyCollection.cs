using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace openhab.net.rest.Core
{
    internal class NotifyCollection<T> : ObservableCollection<T> where T : IOpenhabElement
    {
        Action<T> _callback;

        public NotifyCollection(Action<T> notifyCallback)
        {
            _callback = notifyCallback;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.OldItems.OfType<INotifyPropertyChanged>())
            {
                item.PropertyChanged -= ValueChanged;
            }
            foreach (var item in e.NewItems.OfType<INotifyPropertyChanged>())
            {
                item.PropertyChanged += ValueChanged;
            }
        }

        void ValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender.GetType().Is<T>())
            {
                _callback?.Invoke((T)sender);
            }
        }

        public new bool Contains(T item)
        {
            return this.Any(x => x.Name == item?.Name);
        }

        public T AddOrGet(T item)
        {
            if (Contains(item)) {
                return this.First(x => x.Name == item?.Name);
            }
            else  {
                Add(item);
                return item;
            }
        }
    }
}
