using openhab.net.rest.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest.DataSource
{
    internal interface IDataSource<T> : IDisposable where T : IOpenhabElement
    {
        SiteCollection TargetCollection { get; }

        Task<IEnumerable<T>> GetAll(CancellationToken? token = null);
        Task<IEnumerable<T>> GetAll(MessageHandler message);

        Task<T> GetByName(string name, CancellationToken? token = null);
        Task<T> GetByName(MessageHandler message);

        Task<bool> UpdateState(T element, CancellationToken? token = null);
        Task<bool> UpdateState(MessageHandler message);
    }
}
