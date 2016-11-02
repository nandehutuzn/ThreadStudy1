using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            object lock1 = new object();
            object lock2 = new object();

            new Thread(() => LockTooMuch(lock1, lock2)).Start();

            lock (lock2)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Monitor.TryEnter allows not to get stuck");
                if (Monitor.TryEnter(lock1, TimeSpan.FromSeconds(5)))
                {
                    Console.WriteLine("Acquired a protected resource succesfully");
                }
                else
                    Console.WriteLine("Timeout acquiring a resource!");
            }

            new Thread(() => LockTooMuch(lock1, lock2)).Start();
            lock (lock2)
            {
                Console.WriteLine("This will be a deadlock");
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Console.WriteLine("Acquired a protected resource succesfully");
                }
            }

            Console.ReadKey();
        }

        static void LockTooMuch(object lock1, object lock2)
        {
            lock (lock1)
            {
                Thread.Sleep(1000);
                lock (lock2)
                {
                    Console.WriteLine("LockTooMuch: ID {0}", Thread.CurrentThread.ManagedThreadId);
                }
            }
        }
    }
}
