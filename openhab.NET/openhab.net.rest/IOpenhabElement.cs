using System.ComponentModel;

namespace openhab.net.rest
{
    public interface IOpenhabElement : INotifyPropertyChanged
    {
        string Name { get; }
    }
}
