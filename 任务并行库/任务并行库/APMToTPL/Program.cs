using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace APMToTPL
{
    class Program
    {
        private delegate string AsyncchronousTasks(string threadName);

        private delegate string IncompatibleAsynchronousTask(out int threadID);

        static void Main(string[] args)
        {
            int threadID;
            AsyncchronousTasks d = Test;
            IncompatibleAsynchronousTask e = Test;

            Console.WriteLine("Option 1");
            Task<string> task = Task<string>.Factory.FromAsync(
                d.BeginInvoke("AsyncTaskThead", Callback, "a delegate asynchronous call"), d.EndInvoke);

            task.ContinueWith(t => Console.WriteLine("Callback is finished,now running a continuation! Result: {0}", t.Result));

            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(500);
            }
            Console.WriteLine(task.Status);
            Thread.Sleep(1000);

            Console.WriteLine("--------------------------");
            Console.WriteLine();
            Console.WriteLine("Option 2");

            task = Task<string>.Factory.FromAsync(
                d.BeginInvoke, d.EndInvoke, "AsyncTaskThread", "A delegate asynchronous call");

            task.ContinueWith(t => Console.WriteLine("Task is completed, now running a continuation! Result: {0}", t.Result));

            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(500);
            }
            Console.WriteLine(task.Status);
            Thread.Sleep(1000);

            Console.WriteLine("----------------------------");
            Console.WriteLine();
            Console.WriteLine("Option 3");

            IAsyncResult ar = e.BeginInvoke(out threadID, Callback, "a delegate asynchronous call");
            task = Task<string>.Factory.FromAsync(ar, o => e.EndInvoke(out threadID, ar));

            task.ContinueWith(t => Console.WriteLine("Task is completed, now running a contiuation! Result: {0}, ThreadID: {1}", t.Result, threadID));

            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(500);
            }
            Console.WriteLine(task.Status);

            Console.ReadKey();
        }

        private static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Starting a callback ...");
            Console.WriteLine("State passed to a callback: {0}", ar.AsyncState);
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Thread pool worker thread id: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static string Test(string threadName)
        {
            Console.WriteLine("Start .....");
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(2000);
            return string.Format("Thread name: {0}", threadName);
        }

        private static string Test(out int threadID)
        {
            Console.WriteLine("Starting ...");
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(2000);
            threadID = Thread.CurrentThread.ManagedThreadId;
            return string.Format("Thread pool worker thread id was : {0}", threadID);
        }
    }
}
