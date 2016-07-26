using GalaSoft.MvvmLight;
using openhab.net.rest.Items;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace App1
{
    public class ItemWrapper<T, U> : ObservableObject, IDisposable where T : OpenhabItem
    {
        T _obj;
        U _value;
        bool perform = true;

        public ItemWrapper(T obj)
        {
            _obj = obj;
            _obj.Changed += Update;
            Update(null, null);
        }

        async void Update(object o, EventArgs e)
        {
            if (perform)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (
                    CoreDispatcherPriority.Normal,
                    () => _value = Convert.ChangeType(((dynamic)_obj).Value, typeof(U))
                );
                RaisePropertyChanged(() => Value);
            }
        }

        public U Value
        {
            get { return _value; }
            set
            {
                perform = false;
                Set(ref _value, value);
                ((dynamic)_obj).Value = value;
                perform = true;
            }
        }

        public void Dispose()
        {
            _obj.Changed -= Update;
        }
    }
}
