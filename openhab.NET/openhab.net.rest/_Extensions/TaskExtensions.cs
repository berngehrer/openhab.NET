using System.Threading;
using System.Threading.Tasks;

namespace openhab.net.rest
{
    internal static class TaskExtensions
    {
        public static bool IsSuccess(this Task task)
        {
            if (task == null)
                return false;
            return task.IsCompleted && !task.IsFaulted && !task.IsCanceled; 
        }

        public static T GetResultSave<T>(this Task<T> task)
        {
            return task.IsSuccess() ? task.Result : default(T);
        }

        public static void WaitSave(this Task task, CancellationToken token)
        {
            task.RunSynchronously();
            task.Wait(token);
        }

        public static T WaitSave<T>(this Task<T> task, CancellationToken token)
        {
            task.RunSynchronously();
            task.Wait(token);
            return task.GetResultSave();
        }
    }
}
