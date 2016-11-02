using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 计时器
{
    class Program
    {
        static Timer _timer;

        static void Main(string[] args)
        {
            Console.WriteLine("Press 'Enter' to stop the timer ...");
            DateTime start = DateTime.Now;
            _timer = new Timer(o => TimerOperation(start), null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)); //周期2秒
            Thread.Sleep(TimeSpan.FromSeconds(6));
            _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(4)); //改为周期4s
            Console.ReadKey();
            _timer.Dispose();
        }

        static void TimerOperation(DateTime start)
        {
            TimeSpan elapsed = DateTime.Now - start;
            Console.WriteLine("{0} seconds from {1}.Tiemr thread pool thread id: {2}",
                elapsed.Seconds, start, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
