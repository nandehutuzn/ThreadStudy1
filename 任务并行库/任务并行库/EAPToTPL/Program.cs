using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace EAPToTPL
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcs = new TaskCompletionSource<int>();

            var worker = new BackgroundWorker();
            worker.DoWork += (sender, eventArgs) =>
                {
                    eventArgs.Result = TaskMethod("Background worker", 5);
                };
            worker.RunWorkerCompleted += (sender, eventArgs) =>
                {
                    if (eventArgs.Error != null)
                        tcs.SetException(eventArgs.Error);
                    else if (eventArgs.Cancelled)
                        tcs.SetCanceled();
                    else
                        tcs.SetResult((int)eventArgs.Result);
                };
            worker.RunWorkerAsync();
            tcs.Task.ContinueWith(o =>
                {
                    if (o.IsFaulted)
                        Console.WriteLine("异常: {0}", o.Exception.InnerException.Message);
                    else if (o.IsCompleted)
                        Console.WriteLine("Result is: {0}", o.Result);
                    else if (o.IsCanceled)
                        Console.WriteLine("Canceled !");
                });
            //int result = tcs.Task.Result;
            //Console.WriteLine("Result is: {0}", result);
            Console.ReadKey();
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}, is thread pool thread: {2}", name,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            //throw new Exception("错误");
            return 42 * seconds;
        }
    }
}
