using System;

namespace openhab.net.rest
{
    internal interface IElementObserver : IDisposable
    {
        void Subscribe(IElementObservable obj);
        void Notify(IElementObservable sender, IOpenhabElement element);
    }

    internal interface IElementObservable
    {
        void OnNotify(IOpenhabElement element);
    }
}
