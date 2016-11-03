using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 组合任务
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstTask = new Task<int>(() => TaskMethod("First Task", 3));
            var secondTask = new Task<int>(() => TaskMethod("Second Task", 2));

            firstTask.ContinueWith(
                t => Console.WriteLine("The first answer is {0}. Thread is{1}, is thread pool thread: {2}",
                    t.Result, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

    
            secondTask.Start();
            firstTask.Start();
           

            Thread.Sleep(TimeSpan.FromSeconds(4));
            secondTask.ContinueWith(
       t =>
       {
           Console.WriteLine("The second answer is {0}. Thread is{1}, is thread pool thread: {2}",
           t.Result, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
           int tt = t.Result;
       },
           TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);//直接在主线程中执行,如果得到结果的话
           

            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine();

            firstTask = new Task<int>(() =>
            {
                var innerTask = Task.Factory.StartNew(() => TaskMethod("Child Task", 5), TaskCreationOptions.AttachedToParent);
                innerTask.ContinueWith(t => TaskMethod("Child Task Continue", 2), TaskContinuationOptions.AttachedToParent);
                return TaskMethod("Parent Task", 2);
            });
            firstTask.Start();

            while (!firstTask.IsCompleted)
            {
                Console.WriteLine(firstTask.Status);
                Thread.Sleep(500);
            }
            Console.WriteLine(firstTask.Status);
            Console.ReadKey();
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}, is thread pool: {2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }
    }
}
