using System;

namespace openhab.net.rest
{
    public interface IOpenhabElement 
    {
        event EventHandler Changed;

        string Name { get; }
        bool IsEqual(IOpenhabElement element);
    }
}
