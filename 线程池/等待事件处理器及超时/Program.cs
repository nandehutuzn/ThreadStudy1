using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 等待事件处理器及超时
{
    class Program
    {
        static void Main(string[] args)
        {
            RunOperations(TimeSpan.FromSeconds(5));
            RunOperations(TimeSpan.FromSeconds(7));

            Console.ReadKey();
        }

        static void RunOperations(TimeSpan workerOperationTimeout)
        { 
            using(var evt = new ManualResetEvent(false))
            using (var cts = new CancellationTokenSource())
            {
                Console.WriteLine("Registering timeout operations ...");
                var worker = ThreadPool.RegisterWaitForSingleObject(evt,
                    (state, isTimeOut) => WorkerOperationWait(cts, isTimeOut), null, workerOperationTimeout, true);
                Console.WriteLine("Starting long running operation ...");
                ThreadPool.QueueUserWorkItem(o => WorkerOperation(cts.Token, evt));
                Thread.Sleep(workerOperationTimeout.Add(TimeSpan.FromSeconds(2)));
                worker.Unregister(evt);
                
            }
        }
        
        static void WorkerOperation(CancellationToken token, ManualResetEvent evt)
        {
            for (int i = 0; i < 6; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            evt.Set();//事件处理器发送信号，注册回调方法将被执行
        }

        static void WorkerOperationWait(CancellationTokenSource cts, bool isTimeOut)
        {
            if (isTimeOut)  //指明这个回调操作是超时引起还是  evt.Set() 方法引起
            {
                cts.Cancel();
                Console.WriteLine("Worker operation timed out and was cancled");
            }
            else
                Console.WriteLine("Worker operation succeded");
        }
    }
}
