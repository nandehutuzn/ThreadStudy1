﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentQueue异步
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = RunProgram();
            t.Wait();

            Console.ReadKey();
        }

        static async Task RunProgram()
        {
            var taskQueue = new ConcurrentQueue<CustomTask>();
            var cts = new CancellationTokenSource();

            var taskSource = Task.Run(() => TaskProducer(taskQueue));

            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = i.ToString();
                processors[i - 1] = Task.Run(() => TaskProcessor(taskQueue, "Processor" + processorId, cts.Token));
            }
            await taskSource;
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            await Task.WhenAll(processors);
        }
         
        static async Task TaskProducer(ConcurrentQueue<CustomTask> queue)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask { Id = i };
                queue.Enqueue(workItem);
                Console.WriteLine("Task {0} has been posted", workItem.Id);
            }
        }

        static async Task TaskProcessor(ConcurrentQueue<CustomTask> queue, string name, CancellationToken token)
        {
            CustomTask workItem;
            bool dequeueSuccessful = false;

            await GetRandomDelay();
            do
            {
                dequeueSuccessful = queue.TryDequeue(out workItem);
                if (dequeueSuccessful)
                    Console.WriteLine("Task {0} has been processed by {1}", workItem.Id, name);
                await GetRandomDelay();
            } while (!token.IsCancellationRequested);
        }

        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

        class CustomTask
        {
            public int Id { get; set; }
        }
    }
}
