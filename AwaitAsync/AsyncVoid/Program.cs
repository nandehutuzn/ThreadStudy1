using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AsyncVoid
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncTask();
            t.Wait();

            AsyncVoid();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            t = AsyncTaskWithErrors();
            //t.Wait();  //抛出 AggregiateException 异常
            while (!t.IsFaulted)
                Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine(t.Exception);

            //try
            //{
            //    AsyncVoidWithErrors();
            //    Thread.Sleep(TimeSpan.FromSeconds(3));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //try
            //{
            //    int[] numbers = new[] { 1, 2, 3, 4, 5 };
            //    Array.ForEach(numbers, async number =>
            //        {
            //            await Task.Delay(TimeSpan.FromSeconds(1));
            //            if (number == 3)
            //                throw new FormatException("Boom!");
            //            Console.WriteLine(number);
            //        });
            //}
            //catch  //这种方式没用，因为  Array.ForEach 方法内调用的是 Async Void (Action) ,此异常只能在异常抛出上下文中捕获
            //{
            //    Console.WriteLine();
            //}

            Console.ReadKey();
        }

        async static Task AsyncTaskWithErrors()
        {
            string result = await GetInfoAsync("AsyncTaskException", 2);
            Console.WriteLine(result);
        }

        async static void AsyncVoidWithErrors()
        {
            try
            {
                string result = await GetInfoAsync("AsyncVoidException", 2);
                Console.WriteLine(result);
            }
            catch   //因为返回 Void 所以直接在当前上下文中抛出异常，
            {       //如果 返回 Task 则会将此异常封装抛给调用线程
                Console.WriteLine("");
            }
        }

        async static Task AsyncTask()
        {
            string result = await GetInfoAsync("AsyncTask", 2);
            Console.WriteLine(result);
        }

        private static async void AsyncVoid()
        {
            string result = await GetInfoAsync("AsyncVoid", 2);
            Console.WriteLine(result);
        }

        async static Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            if (name.Contains("Exception"))
                throw new Exception(string.Format("Boom from {0}", name));
            return string.Format("Task {0} is running on a thread id {1}, is thread pool thread: {2}", name,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
