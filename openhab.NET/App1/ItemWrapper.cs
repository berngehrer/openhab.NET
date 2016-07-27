using GalaSoft.MvvmLight;
using openhab.net.rest.Items;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace App1
{
    public class ItemWrapper<T> : ObservableObject, IDisposable where T : OpenhabItem
    {
        T _item;
        public ItemWrapper(T item)
        {
            _item = item;
            _item.Changed += Changed;
        }
        
        public object Value
        {
            get { return _item.ToNative(); }
            set { _item.FromNative(value); }
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
            _item.Changed -= Changed;
        }

        void Changed(object sender, EventArgs e) => RaiseChanged();


        public static implicit operator ItemWrapper<T>(OpenhabItem item)
        {
            return new ItemWrapper<T>((T)item);
        }
    }
}
