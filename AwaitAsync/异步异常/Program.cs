using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 异步异常
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronousProcessing();
            t.Wait();

            Console.ReadKey();
        }

        async static Task AsynchronousProcessing()
        {
            Console.WriteLine("1. Single exception");

            try
            {
                string result = await GetInfoAsync("Task 1", 2);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exeeption details: {0}", ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("2. Multiple exceptions");

            Task<string> t1 = GetInfoAsync("Task 1", 3);
            Task<string> t2 = GetInfoAsync("Task 2", 2);
            try
            {
                string[] results = await Task.WhenAll(t1, t2);
                Console.WriteLine(results.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception details: {0}", ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("2. Multiple exceptions with AggregateException");

            t1 = GetInfoAsync("Task 1", 3);
            t2 = GetInfoAsync("Task 2", 2);
            Task<string[]> t3 = Task.WhenAll(t1, t2);
            try
            {
                string[] results = await t3;
                Console.WriteLine(results.Length);
            }
            catch
            {
                var ac = t3.Exception.Flatten();
                var exceptions = ac.InnerExceptions;
                Console.WriteLine("Exceptions caught: {0}", exceptions.Count);
                foreach (var e in exceptions)
                {
                    Console.WriteLine("Exception detail: {0}", e.Message);
                    Console.WriteLine();
                }
            }
        }

        static async Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            throw new Exception(string.Format("Boom from {0}", name));
        }
    }
}
