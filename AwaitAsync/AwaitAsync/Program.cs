using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AwaitAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncchronyWithTPL();
            t.Wait();

            t = AsyncchronyWithAwait();
            t.Wait();

            Console.WriteLine("End .........");
            Console.ReadKey();
        }

        static Task AsyncchronyWithTPL()
        {
            Task<string> t = GetInfoAsync("Task 1");
            Task t2 = t.ContinueWith(task => Console.WriteLine(t.Result),
                TaskContinuationOptions.NotOnFaulted);
            Task t3 = t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException),
                TaskContinuationOptions.OnlyOnFaulted);

            return Task.WhenAny(t2, t3);
        }

        async static Task AsyncchronyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Task 2");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        async static Task<string> GetInfoAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            //throw new Exception("Boom!");

            return string.Format("Task {0} is running on a thead id {1}, is thread pool thread: {2}", name
                , Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
