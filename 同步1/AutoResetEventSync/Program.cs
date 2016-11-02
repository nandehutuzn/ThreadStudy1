using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AutoResetEventSync
{
    class Program
    {
        private static AutoResetEvent _workerEvent = new AutoResetEvent(false);  //内核时间模式   等待时间不能太长
        private static AutoResetEvent _mainEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            new Thread(() => Test("Thread 1")).Start();
            new Thread(() => Test("Thread 2")).Start();
            new Thread(() => Test("Thread 3")).Start();

            Thread.Sleep(5000);
            _mainEvent.Set(); //一次只允许一个通过
            Thread.Sleep(100);
            _mainEvent.Set();
            Thread.Sleep(100);
            _mainEvent.Set();

            //var t = new Thread(() => Process(10));
            //t.Start();

            //Console.WriteLine("Waiting for another thread to complete work ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            //_workerEvent.WaitOne();
            //Console.WriteLine("First operation is completed! ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            //Console.WriteLine("Performing an operation on a main thread ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            //Thread.Sleep(5000);
            //_mainEvent.Set();
            //Console.WriteLine("Now running the second operation on a second thread ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            //_workerEvent.WaitOne();
            //Console.WriteLine("Second operation is completed ,ID {0}", Thread.CurrentThread.ManagedThreadId);

            Console.ReadKey();
        }

        static void Process(int seconds)
        {
            Console.WriteLine("Starting a long running work ...... ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine("Work is Done!  ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            _workerEvent.Set();
            Console.WriteLine("Waiting for a main thread to complete its work ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            _mainEvent.WaitOne();
            Console.WriteLine("Starting second operation ...... ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine("Work is done! ,ID {0}", Thread.CurrentThread.ManagedThreadId);
            _workerEvent.Set();
        }

        static void Test(string threadName)
        {
            Console.WriteLine("{0} Start Wait", threadName);
            _mainEvent.WaitOne();
            Console.WriteLine("{0} Enter ........", threadName);
        }
    }
}
