using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 任务异常
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> task;
            try
            {
                task = Task.Run(() => TaskMethod("Task 1", 2));
                int result = task.Result;  //将异常封装进 AggregateException ，可访问其InnerException属性得到来自底层的异常
                Console.WriteLine("Result: {0}", result);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Exception: {0}", ex);  
                
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine();

            try
            {
                task = Task.Run(() => TaskMethod("Task 2", 2));
                int result = task.GetAwaiter().GetResult();//不对异常进行封装
                Console.WriteLine("Result: {0}", result);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Exception Caught: {0}", ex);
            }
            Console.WriteLine("----------------------------");
            Console.WriteLine();

            var t1 = new Task<int>(() => TaskMethod("Task 3", 3));
            var t2 = new Task<int>(() => TaskMethod("Task 4", 2));
            var complexTask = Task.WhenAll(t1, t2);
            var exceptionHandler = complexTask.ContinueWith(t => Console.WriteLine("Exception caught: {0}", t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            t1.Start();
            t2.Start();

            Console.ReadKey();
        }

        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}, is Thread pool thread: {2}", name
                , Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new FormatException ("Boom!");
            return 42 * seconds;
        }
    }
}
