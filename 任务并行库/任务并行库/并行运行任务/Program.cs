using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 并行运行任务
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstTask = new Task<int>(() => TaskMethod("First Task", 3));
            var secondTask = new Task<int>(() => TaskMethod("Second Task", 2));
            var whenAllTask = Task.WhenAll(firstTask, secondTask);

            whenAllTask.ContinueWith(t => Console.WriteLine("The first answer is {0}, the second is {1}", t.Result[0], t.Result[1]), TaskContinuationOptions.OnlyOnRanToCompletion);

            firstTask.Start();
            secondTask.Start();

            Thread.Sleep(4000);
            var tasks = new List<Task<int>>();
            for (int i = 1; i < 4; i++)
            {
                int counter = i;
                var task = new Task<int>(() => TaskMethod(string.Format("Task {0}", counter), counter));
                tasks.Add(task);
                task.Start();
            }

            while (tasks.Count > 0)
            {
                var completedTask = Task.WhenAny(tasks).Result;
                tasks.Remove(completedTask);
                Console.WriteLine("A task has been completed with result {0}", completedTask.Result);
            }

            Console.ReadKey();
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}, is thread pool thread: {2}", name,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }
    }
}
