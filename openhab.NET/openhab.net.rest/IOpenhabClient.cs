using System.Collections.Generic;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public interface IOpenhabClient<T> where T : IOpenhabElement
    {
        /// <summary>
        /// Returns a single item or single page
        /// </summary>
        Task<T> GetByNameAsync(string name);

        /// <summary>
        /// Returns all items from this collection
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();
    }
}
