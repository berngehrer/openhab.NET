using System.ComponentModel;

namespace openhab.net.rest.DataSource
{
    public interface IOpenhabElement : INotifyPropertyChanged
    {
        string Name { get; }
        bool IsEqual(IOpenhabElement element);
    }
}
