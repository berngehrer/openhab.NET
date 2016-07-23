using System.Collections.Generic;
using System.Threading.Tasks;

namespace openhab.net.rest.DataSource
{
    internal interface IDataSource<T> where T : IOpenhabElement
    {
        Task<IEnumerable<T>> GetAll(OpenhabClient client);
        Task<T> GetByName(OpenhabClient client, string name);
    }
}
