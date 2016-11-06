using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLoggerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
             Go();

            Console.ReadKey();
        }

        private static async Task Go()
        {
            TaskLogger.LogLevel = TaskLogger.TaskLogLevel.Pending;
            var tasks = new List<Task>
            {
                Task.Delay(2000).Log("2s op"),
                Task.Delay(5000).Log("5s op"),
                Task.Delay(6000).Log("6s op"),
            };

            foreach (var op in TaskLogger.GetLogEntries().OrderBy(tle => tle.LogTime))
                Console.WriteLine(op);
        }
    }
}
