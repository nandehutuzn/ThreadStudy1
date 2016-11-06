using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 total = DirectoryBytes(@"E:\360云盘资料\1.算法设计", "*.pdf", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Total Length: {0}", total);
            Console.ReadKey();
        }

        static Int64 DirectoryBytes(string path, string searchPattern, SearchOption searchOption)
        {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            Int64 masterTotal = 0;

            ParallelLoopResult result = Parallel.ForEach<string, Int64>(files,
                () => { return 0; },
                (file, loopState, index, taskLoclTotal) =>
                {
                    if (Path.GetFileName(file) == "编程之美.pdf")
                        loopState.Break();
                    Int64 fileLength = 0;
                    FileStream fs = null;
                    try
                    {
                        fs = File.OpenRead(file);
                        fileLength = fs.Length;
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Dispose();
                    }
                    return taskLoclTotal + fileLength;
                },
                taskLocalTotal =>
                {
                    Interlocked.Add(ref masterTotal, taskLocalTotal);
                });
            //Thread.Sleep(5000);
            return masterTotal;
        }
    }
}
