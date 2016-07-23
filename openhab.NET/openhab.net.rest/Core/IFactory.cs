
namespace openhab.net.rest.Core
{
    internal interface IFactory
    {
        object Create();
    }

    internal interface IFactory<out T> : IFactory
    {
        new T Create();
    }
}
