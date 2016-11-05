using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace ConcurrentBag爬虫
{
    class Program
    {
        static Dictionary<string, string[]> _contentEmulation = new Dictionary<string, string[]>();

        static void Main(string[] args)
        {
            CreateLinks();
            Task t = RunProgram();
            t.Wait();

            Console.ReadKey();
        }

        static async Task RunProgram()
        {
            var bag = new ConcurrentBag<CrawlingTask>();

            string[] urls = new[]{"http://microsoft.com/",
            "http://google.com/", "http://facebook.com/", "http://twitter.com/"};

            var crawles = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string crawlerName = "Crawler " + i.ToString();
                bag.Add(new CrawlingTask { UrlToCrawl = urls[i - 1], ProducerName = "root" });
                crawles[i - 1] = Task.Run(() => Crawl(bag, crawlerName));
            }

            await Task.WhenAll(crawles);
        }

        static async Task Crawl(ConcurrentBag<CrawlingTask> bag, string crawlerName)
        {
            CrawlingTask task;
            while (bag.TryTake(out task))
            {
                IEnumerable<string> urls = await GetLinksFromContent(task);
                if (urls != null)
                {
                    foreach (var url in urls)
                    {
                        var t = new CrawlingTask
                        {
                            UrlToCrawl = url,
                            ProducerName = crawlerName,
                        };
                        bag.Add(t);
                    }
                }
                Console.WriteLine("Indexing url {0} posted by {1} is completed by {2}",
                    task.UrlToCrawl, task.ProducerName, crawlerName);
            }
        }

        static async Task<IEnumerable<string>> GetLinksFromContent(CrawlingTask task)
        {
            await GetRandowDelay();

            if (_contentEmulation.ContainsKey(task.UrlToCrawl))
                return _contentEmulation[task.UrlToCrawl];
            return null;
        }

        static void CreateLinks()
        {
            _contentEmulation["http://microsoft.com/"] = new[] {"http://microsoft.com/a.html", 
            "http://microsoft.com/b.html"};
            _contentEmulation["http://microsoft.com/a.html"] = new[] {"http://microsoft.com/c.html", 
            "http://microsoft.com/d.html"};
            _contentEmulation["http://microsoft.com/b.html"] = new[] {"http://microsoft.com/e.html"};
        }

        static Task GetRandowDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(150, 200);
            return Task.Delay(delay);
        }

        class CrawlingTask
        {
            public string UrlToCrawl { get; set; }

            public string ProducerName { get; set; }
        }
    }
}
