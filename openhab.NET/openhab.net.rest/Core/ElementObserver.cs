using openhab.net.rest.DataSource;
using System;
using System.Collections.Generic;

namespace openhab.net.rest.Core
{
    internal interface IElementObserver : IDisposable
    {
        void Subscribe(IElementObservable obj);
        void Notify(IElementObservable sender);
        void Notify(IElementObservable sender, IOpenhabElement element);
    }

    internal interface IElementObservable
    {
        void OnNotify(IOpenhabElement element);
    }


    internal class ElementObserver : IElementObserver
    {
        List<IElementObservable> _subscriber = new List<IElementObservable>();

        public void Notify(IElementObservable sender, IOpenhabElement element)
        {
            foreach (var obj in _subscriber)
            {
                if (!Object.ReferenceEquals(obj, sender)) {
                    obj.OnNotify(element);
                }
            }
        }

        public void Notify(IElementObservable sender)
        {
            Notify(sender, sender as IOpenhabElement);
        }

        public void Subscribe(IElementObservable obj)
        {
            if (!_subscriber.Contains(obj))
                _subscriber.Add(obj);
        }

        public void Dispose()
        {
            _subscriber.Clear();
        }
    }
}
