using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AwaitInLambda
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronousProcessing();
            t.Wait();

            Console.ReadKey();
        }

        async static Task AsynchronousProcessing()
        {
            Func<string, Task<string>> asyncLambda = async name =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    return string.Format("Task {0} is running on a thread id {1}, is thread pool thread: {2}", name,
                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
                };

            string result = await asyncLambda("async lambda");

            Console.WriteLine(result);
        }
    }
}
