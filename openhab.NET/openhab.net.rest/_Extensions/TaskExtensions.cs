using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal static class TaskExtensions
    {
        public static T WaitSynchronously<T>(this Task<T> task, CancellationToken token)
        {
            task.RunSynchronously();
            task.Wait(token);
            return task.GetAwaiter().GetResult();
        }
    }
}
