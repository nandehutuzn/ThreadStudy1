using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BarrierSync
{
    class Program
    {
        static Barrier _barrier = new Barrier(2,
            b => Console.WriteLine("End of phase{0}, ID: {1}", b.CurrentPhaseNumber + 1, Thread.CurrentThread.ManagedThreadId));

        static void Main(string[] args)
        {
            Console.WriteLine("Main ID: {0}", Thread.CurrentThread.ManagedThreadId);
            new Thread(() => PlayMusic("The qutarist", "Play an amizing solo", 5)).Start();
            new Thread(() => PlayMusic("The singer", "sing his song", 2)).Start();

            Console.ReadKey();
        }

        static void PlayMusic(string name, string message, int seconds)
        {
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("---------------------------------");
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                Console.WriteLine("{0} starts to {1}, ID:{2}", name, message, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                
                _barrier.SignalAndWait();
                Console.WriteLine("{0} finished to {1}, ID: {2}", name, message, Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
