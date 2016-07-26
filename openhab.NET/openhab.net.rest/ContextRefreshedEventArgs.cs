using openhab.net.rest.DataSource;
using System;

namespace openhab.net.rest
{
    public delegate void ContextRefreshedHandler<T>(object sender, ContextRefreshedEventArgs<T> args) where T : IOpenhabElement;

    public class ContextRefreshedEventArgs<T> : EventArgs where T : IOpenhabElement
    {
        public ContextRefreshedEventArgs(T element)
        {
            Element = element;
        }
        public T Element { get; }
    }
}
