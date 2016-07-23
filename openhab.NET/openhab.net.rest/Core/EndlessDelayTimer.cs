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
            for (;;)
            {
                Task.Delay(_delay, Token).ContinueWith(_ => callback?.Invoke(_state),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default
                );
                if (IsCancellationRequested) {
                    break;
                }
            }
        }

        public new void Dispose()
        {
            base.Cancel();
            _state?.Dispose();
        }
    }

    internal sealed class ClientBackgroundWorker : EndlessDelayTimer<OpenhabClient>
    {
        public ClientBackgroundWorker(OpenhabClient state, int delay)  : base(state, delay) { }
    }
}
