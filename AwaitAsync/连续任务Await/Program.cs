using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 连续任务Await
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronyWithTPL();
            Console.WriteLine("Main Back1");
            t.Wait();

            t = AsynchronyWithAwait();
            Console.WriteLine("Main Back2");
            t.Wait();

            Console.ReadKey();
        }

        static Task AsynchronyWithTPL()
        {
            var containerTask = new Task(() =>
                {
                    Task<string> t = GetInfoAsync("TPL 1");
                    t.ContinueWith(task =>
                        {
                            Console.WriteLine(t.Result);
                            Task<string> t2 = GetInfoAsync("TPL 2");
                            t2.ContinueWith(innerTask => Console.WriteLine(innerTask.Result), TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);

                            t2.ContinueWith(innerTask => Console.WriteLine(innerTask.Exception.InnerException),
                                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
                        }, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);

                    t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException),
                        TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
                });
            containerTask.Start();
            return containerTask;
        }

        static async Task AsynchronyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Async 1");
                Console.WriteLine(result);
                result = await GetInfoAsync("Async 2");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            Console.WriteLine("Task {0} started!", name);
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (name == "TPL 2")
                throw new Exception("Boom!");
            return string.Format("Task {0} is running on a thread id {1},is thread pool thread: {2}", name,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
