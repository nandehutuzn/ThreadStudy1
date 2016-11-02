using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(Print);
            t.Start();
            //Print();
            Thread.Sleep(TimeSpan.FromSeconds(4));
            t.Abort();
            Console.WriteLine("T has aborted");
            
            Console.ReadKey();
        }

        static void Print()
        {
            try
            {
                Console.WriteLine("Start................ID: {0}", Thread.CurrentThread.ManagedThreadId);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i.ToString());
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                Console.WriteLine("End ..................ID: {0}", Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                Thread.ResetAbort(); //拒绝被终止
                Console.WriteLine("End ..................ID: {0}", Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
