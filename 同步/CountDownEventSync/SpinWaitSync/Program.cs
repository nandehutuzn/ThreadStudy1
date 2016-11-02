using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SpinWaitSync
{
    class Program
    {
        static volatile bool _isCompleted = false;

        static void Main(string[] args)
        {
            var t1 = new Thread(UserModeWait);
            var t2 = new Thread(HybridSpinWait);

            Console.WriteLine("Running user mode waiting");
            t1.Start();
            Thread.Sleep(1000);
            _isCompleted = true;
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _isCompleted = false;
            Console.WriteLine("Running hybrid SpinWait construct waiting");
            t2.Start();
            Thread.Sleep(50);
            _isCompleted = true;
            Console.ReadKey();
        }

        static void UserModeWait()
        {
            while (!_isCompleted)
            {
                Console.Write(".");
            }
            Console.WriteLine();
            Console.WriteLine("UserMode Waiting is complete");
        }

        static void HybridSpinWait()
        {
            var w = new SpinWait();
            while (!_isCompleted)
            {
                w.SpinOnce();//如果不调用这句这一直不切换为阻塞状态
                Console.WriteLine(w.NextSpinWillYield);
            }
            Console.WriteLine("HybridSpin Waiting is complete");
        }
    }
}
