using System;
using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest.Core
{
    internal class EndlessDelayTimer<T> : CancellationTokenSource, IDisposable where T : IDisposable
    {
        T _state;
        int _delay;

        public EndlessDelayTimer(T state, int delay)
        {
            _state = state;
            _delay = delay;
        }

        public void Start(Action<T> callback)
        {
            Task.Run(() =>
            {
                for (;;)
                {
                    Task.Delay(_delay, Token).Wait();
                    callback.Invoke(_state);
                    if (IsCancellationRequested) {
                        break;
                    }
                }
            },
            Token);
        }

        public new void Dispose()
        {
            base.Cancel();
            _state?.Dispose();
        }
    }


    internal sealed class BackgroundClient : EndlessDelayTimer<OpenhabClient>
    {
        public BackgroundClient(OpenhabClient state, int delay) : base(state, delay) { }
    }
}
