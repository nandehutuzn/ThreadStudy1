using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;

namespace 自定义AwaitAble
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
            var sync = new CustomAwaitable(true);
            string result = await sync;
            Console.WriteLine(result);

            var async = new CustomAwaitable(false);
            result = await async;
            Console.WriteLine(result);
        }

        class CustomAwaitable
        {
            private readonly bool _completeSynchronously;

            public CustomAwaitable(bool completeSynchronously)
            {
                _completeSynchronously = completeSynchronously;
            }

            public CustomAwaiter GetAwaiter()
            {
                return new CustomAwaiter(_completeSynchronously);
            }
        }

        class CustomAwaiter : INotifyCompletion
        {
            private string _result = "Completed synchronously";
            private readonly bool _completeSynchronously;

            public bool IsCompleted { get { return _completeSynchronously; } }

            public string GetResult() { return _result; }

            public CustomAwaiter(bool completeSyncchronously)
            {
                _completeSynchronously = completeSyncchronously;
            }


            public void OnCompleted(Action continuation)
            {
                ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        _result = GetInfo();
                        if (continuation != null)
                            continuation();
                    });
            }

            private string GetInfo()
            {
                return string.Format("Task is running on a thread id {0}, is thread pool thread； {1}",
                    Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            }
        }
    }
}
