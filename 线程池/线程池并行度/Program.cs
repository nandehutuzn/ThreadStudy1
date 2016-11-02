using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace 线程池并行度
{
    class Program
    {
        static void Main(string[] args)
        {
            const int numberOfOperations = 500;
            long time1, time2;
            var sw = new Stopwatch();
            sw.Start();
            UseThreads(numberOfOperations);
            sw.Stop();
            time1 = sw.ElapsedMilliseconds;
            Console.WriteLine("Execution time using threads: {0}", sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            UseThreadPool(numberOfOperations);
            sw.Stop();
            time2 = sw.ElapsedMilliseconds;
            Console.WriteLine("Execution time using threads: {0}", time1);
            Console.WriteLine("Execution time using pool threads: {0}", time2);

            Console.ReadKey();
        }

        static void UseThreads(int numberOfOperations)
        {
            using (var countdown = new CountdownEvent(numberOfOperations))
            {
                Console.WriteLine("Scheduling work by creating threads");
                for (int i = 0; i < numberOfOperations; i++)
                {
                    var thread = new Thread(() =>
                        {
                            Console.WriteLine("ID: {0}", Thread.CurrentThread.ManagedThreadId);
                            Thread.Sleep(TimeSpan.FromSeconds(0.1));
                            countdown.Signal();
                        });
                    thread.Start();
                }
                countdown.Wait();
                Console.WriteLine();
            }
        }

        static void UseThreadPool(int numberOfOperations)
        {
            using (var countdown = new CountdownEvent(numberOfOperations))
            {
                Console.WriteLine("Starting work on a threadpool");
                for (int i = 0; i < numberOfOperations; i++)
                {
                    ThreadPool.QueueUserWorkItem(o =>
                        {
                            Console.WriteLine("Pool  ID: {0}", Thread.CurrentThread.ManagedThreadId);
                            Thread.Sleep(TimeSpan.FromSeconds(0.1));
                            countdown.Signal();
                        });
                }
                countdown.Wait();
                Console.WriteLine();
            }
        }
    }
}
