using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 并行异步任务Await
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
            Task<string> t1 = GetInfoAsync("Task 1", 3);
            Task<string> t2 = GetInfoAsync("Task 2", 5);

            string[] results = await Task.WhenAll(t1, t2);
            foreach (string result in results)
                Console.WriteLine(result);
        }

        async static Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));//启用计时器后立即返回，因此并行是能看到使用的是同一个线程
            //await Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(seconds)));/这里会阻塞线程几秒，所以会在线程池中启用新的线程
            return string.Format("Task {0} is running on a thread id {1}, is thread pool thread: {2}", name,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
