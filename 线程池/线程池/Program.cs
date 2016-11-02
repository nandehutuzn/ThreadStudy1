using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 线程池
{
    //线程池用途是执行运行时间短的操作，长时间测操作new 一个 Thread
    class Program
    {
        private delegate string RunOnThreadPool(out int threadID);
        static void Main(string[] args)
        {
            int threadID = 0;
            RunOnThreadPool poolDelegate = Test;
            //var t = new Thread(() => Test(out threadID));
            //t.Start();
            //t.Join();
            //Console.WriteLine("Thread id: {0}", threadID);

            IAsyncResult r = poolDelegate.BeginInvoke(out threadID, Callback, "a delegate asyncchronous call");
            r.AsyncWaitHandle.WaitOne(); //这一句可不用，调用EndInvoke 事实上会等待异步操作完成
            try
            {
                string result = poolDelegate.EndInvoke(out threadID, r); //该方法会将任何未处理的异常抛回到调用线程中
                Console.WriteLine("Thread pool thread id: {0}", threadID);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Start a callback .........");
            Console.WriteLine("State passed to a callback: {0}", ar.AsyncState); //该属性为用户自定义传入该回调函数的参数
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Thread pool worker thread id: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static string Test(out int threadID)
        {
            //try
            {
                Console.WriteLine("Start .......ID: {0}", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
                Thread.Sleep(TimeSpan.FromSeconds(2));
                threadID = Thread.CurrentThread.ManagedThreadId;
                throw new Exception("Test");
                return string.Format("Thread pool worker thread id was: {0}", threadID);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message + " Test");
            //    threadID = 0;
            //    return "";
            //}
        }
    }
}
