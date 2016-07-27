using GalaSoft.MvvmLight;
using openhab.net.rest.Items;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace App1
{
    public class ItemWrapper<T> : ObservableObject, IDisposable where T : OpenhabItem
    {
        public ItemWrapper(T item)
        {
            Item = item;
            Item.Changed += Changed;
        }
        
        public T Item { get; }

        public object Value
        {
            get { return Item.ToNative(); }
            set { Item.FromNative(value); }
        }

        async void RaiseChanged()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
            (
                CoreDispatcherPriority.Normal,
                () => RaisePropertyChanged(() => Value)
            );
        }

        public void Dispose()
        {
            Item.Changed -= Changed;
        }

        void Changed(object sender, EventArgs e) => RaiseChanged();


        public static implicit operator ItemWrapper<T>(OpenhabItem item)
        {
            return new ItemWrapper<T>((T)item);
        }
    }
}
