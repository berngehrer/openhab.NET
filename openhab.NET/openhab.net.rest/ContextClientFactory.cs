using openhab.net.rest.Core;
using System;

namespace openhab.net.rest
{
    internal class ContextClientFactory<T> : IFactory<OpenhabClient> where T : IOpenhabElement
    {
        OpenhabContext<T> _context;
        
        public ContextClientFactory(OpenhabContext<T> context)
        {
            _context = context;
        }
        
        public OpenhabClient Create(UpdateStrategy strategy)
        {
            // Timed update
            if (strategy.Interval != TimeSpan.Zero)
            {
                return new OpenhabClient(_context.Connection.Settings);
            }
            // Permanent update by server push
            else if (strategy.Realtime)
            {
                return new OpenhabClient(_context.Connection.Settings, pooling: true);
            }
            // No background update
            return null;
        }

        public OpenhabClient Create()
        {
            return Create(_context.Strategy ?? UpdateStrategy.Default);
        }

        object IFactory.Create() => Create();
    }
}
