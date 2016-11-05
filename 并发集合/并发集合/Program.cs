﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace 并发集合
{
    class Program
    {
        const string Item = "Dictionary item";
        public static string CurrentItem;
        static void Main(string[] args)
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();
            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }
            sw.Stop();
            Console.WriteLine("Writing to dictionary with a lock: {0}", sw.Elapsed);

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                concurrentDictionary[i] = Item;
            }
            sw.Stop();
            Console.WriteLine("Writing to a concurrent dictionary: {0}", sw.Elapsed);

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }
            sw.Stop();
            Console.WriteLine("Reading from dictioary with a lock: {0}", sw.Elapsed);

            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }
            sw.Stop();
            Console.WriteLine("Reading from a concurrent dictionary: {0}", sw.Elapsed);

            Console.ReadKey();
        }
    }
}
