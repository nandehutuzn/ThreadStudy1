using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 任务取消
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            //传递两次Token原因: 如果在任务启动前取消它，该任务的TPL基础设施有责任处理该取消操作，因为这些代码根本不会执行
            var longTask = new Task<int>(() => TaskMethod("Task 1", 10, cts.Token), cts.Token);
            Console.WriteLine(longTask.Status);
            cts.Cancel();
            Console.WriteLine(longTask.Status);
            Console.WriteLine("First task has been cancelled before execution");

            cts = new CancellationTokenSource();
            longTask = new Task<int>(() => TaskMethod("Task 2", 10, cts.Token), cts.Token);
            longTask.Start();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine(longTask.Status);
            }
            cts.Cancel();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(longTask.Status);  //取消后状态仍然为RanToCompletion
            }
            Console.WriteLine("A task has been completed with result {0}", longTask.Result);

            Console.ReadKey();
        }

        static int TaskMethod(string name, int seconds, CancellationToken token)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}, is Thread pool thread: {2}", name
                , Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if (token.IsCancellationRequested)
                    return -1;
            }
            return 42 * seconds;
        }
    }
}
