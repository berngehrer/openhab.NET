﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    public abstract class OpenhabContext<T> : IDisposable where T : IOpenhabElement
    {
        ObjectStateManager<T> _objectManager;
        CancellationTokenSource _cancelSource;

        public event ContextRefreshedHandler<T> Refreshed;


        protected OpenhabContext(ClientSettings settings, UpdateStrategy strategy)
        {
            ClientFactory = new ContextClientFactory(settings, strategy);  
        }
        

        internal ObjectStateManager<T> ObjectManager
        {
            get { return _objectManager ?? (_objectManager = new ObjectStateManager<T>(this)); }
        }

        internal ContextClientFactory ClientFactory { get; }


        public async Task<IEnumerable<T>> GetAll()
        {
            _cancelSource = new CancellationTokenSource();
            return await ObjectManager.ElementSource.GetAll(_cancelSource.Token);
        }

        public async Task<T> GetByName(string name)
        {
            _cancelSource = new CancellationTokenSource();
            return await ObjectManager.ElementSource.GetByName(name, _cancelSource.Token);
        }

        internal void FireRefreshed(IOpenhabElement element)
        {
            if (element?.GetType().Is<T>() ?? false) {
                Refreshed?.Invoke(this, new ContextRefreshedEventArgs<T>((T)element));
            }
        }

        public void Cancel()
        {
            if (!_cancelSource?.IsCancellationRequested ?? false)
            {
                _cancelSource.Cancel(false);
            }
        }

        public void Dispose()
        {
            _cancelSource?.Cancel(false);
            _objectManager.Dispose();
        }
    }
}
