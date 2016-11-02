using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SemaphoreSlimSync
{
    class Program
    {
        static SemaphoreSlim _semaphore = new SemaphoreSlim(4); //只能同时4个线程访问
        static void Main(string[] args)
        {
            for (int i = 0; i <= 6; i++)
            {
                string threadName = "Thread " + i;
                int secondsToWait = 2 + i * 2;
                var t = new Thread(() => AccessDatabase(threadName, secondsToWait));
                t.Start();
            }

            Console.ReadKey();
        }

        static void AccessDatabase(string name, int seconds)
        {
            Console.WriteLine("{0} waits to access a database", name);
            _semaphore.Wait();
            Console.WriteLine("{0} was granted an access to a database", name);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine("{0} is completed", name);
            _semaphore.Release();
        }
    }
}
