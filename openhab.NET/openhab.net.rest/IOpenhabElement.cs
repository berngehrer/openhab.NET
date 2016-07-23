using System.ComponentModel;

namespace openhab.net.rest
{
    public interface IOpenhabElement : INotifyPropertyChanged
    {
        string Name { get; }
        bool HasChanged { get; }
        void Reset();
    }
}
