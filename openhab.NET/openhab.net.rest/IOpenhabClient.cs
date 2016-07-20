using System.Collections.Generic;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public interface IOpenhabClient<T> where T : IOpenhabElement
    {
        Task<T> GetByNameAsync(string name);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
