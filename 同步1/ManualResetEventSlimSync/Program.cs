using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ManualResetEventSlimSync
{
    class Program
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);
        static void Main(string[] args)
        {
            var t1 = new Thread(() => TravelThroughGates("Thread 1", 5));
            var t2 = new Thread(() => TravelThroughGates("Thread 2", 6));
            var t3 = new Thread(() => TravelThroughGates("Thread 3", 12));

            t1.Start();
            t2.Start();
            t3.Start();
            Thread.Sleep(TimeSpan.FromSeconds(6));
            Console.WriteLine("The Gates are now open!");
            _mainEvent.Set();  //开锁
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _mainEvent.Reset();  //加锁
            Console.WriteLine("The Gates have been clsoed");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("The gates are now open for the second time!");
            _mainEvent.Set();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("The gates have been closed!");
            _mainEvent.Reset();

            Console.ReadKey();
        }

        static void TravelThroughGates(string threadName, int seconds)
        {
            Console.WriteLine("{0} falls to sleep", threadName);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine("{0} waits for the gates to open!", threadName);
            _mainEvent.Wait();
            Console.WriteLine("{0} Enters the gates!", threadName);
        }

    }
}
