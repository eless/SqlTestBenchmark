using BenchmarkDotNet.Attributes;
using DatabaseEF31;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SqlTestBenchmark31.Tests
{
    public class SplitToChunks
    {
        private List<long> data;
        private const int SIZE = 65000;

        [GlobalSetup]
        public void GlobalSetup()
        {
            data = Enumerable.Range(0, 100000).Select(v => (long)v).ToList();
        }

        [Benchmark]
        public List<List<long>> GetByArrayToList()
        {
            long[] buffer;
            var result = new List<List<long>>();
            var array = data.ToArray();
            for (int i = 0; i < array.Length; i += SIZE)
            {
                if (array.Length - i < SIZE)
                {
                    buffer = new long[array.Length - i];
                    Array.Copy(array, i, buffer, 0, array.Length - i);
                } else
                {
                    buffer = new long[SIZE];
                    Array.Copy(array, i, buffer, 0, SIZE);
                }
                result.Add(buffer.ToList());
            }
            return result;
        }

        [Benchmark]
        public long[][] GetByArrayToArray()
        {
            long[] buffer;
            long[][] result = new long[data.Count / SIZE + 1][];
            var array = data.ToArray();
            int index = 0;
            for (int i = 0; i < array.Length; i += SIZE)
            {
                if (array.Length - i < SIZE)
                {
                    buffer = new long[array.Length - i];
                    Array.Copy(array, i, buffer, 0, array.Length - i);
                }
                else
                {
                    buffer = new long[SIZE];
                    Array.Copy(array, i, buffer, 0, SIZE);
                }
                result[index] = buffer;
                index++;
            }
            return result;
        }

        [Benchmark]
        public List<List<long>> GetByGroupingToList()
        {
            int i = 0;
            return data.GroupBy(s => i++ / SIZE).Select(s => s.ToList()).ToList();
        }
    }
}
