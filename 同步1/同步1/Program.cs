using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 同步1
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Counter();
            Console.WriteLine("Incorrect counter");
            var t1 = new Thread(() => TestCounter(c));
            var t2 = new Thread(() => TestCounter(c));
            var t3 = new Thread(() => TestCounter(c));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Total Count； {0}", c.Count);
            Console.WriteLine("Correct Counter");
            var c1 = new CounterNoLock();
            t1 = new Thread(() => TestCounter(c1));
            t2 = new Thread(() => TestCounter(c1));
            t3 = new Thread(() => TestCounter(c1));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Total Count； {0}", c1.Count);

            Console.ReadKey();
        }

        static void TestCounter(CounterBase c)
        {
            for (int i = 0; i < 1000000; i++)
            {
                c.Increment();
                c.Decrement();
            }
        }

        abstract class CounterBase
        {
            public abstract void Increment();
            public abstract void Decrement();
        }

        class Counter : CounterBase
        {
            private int _counter;
            public int Count { get { return _counter; } }

            public override void Increment()
            {
                _counter++;
            }

            public override void Decrement()
            {
                _counter--;
            }
        }

        class CounterNoLock : CounterBase
        {
            private int _count;
            public int Count { get { return _count; } }

            public override void Increment()
            {
                Interlocked.Increment(ref _count);
            }

            public override void Decrement()
            {
                Interlocked.Decrement(ref _count);
            }
        }
    
    }
}
