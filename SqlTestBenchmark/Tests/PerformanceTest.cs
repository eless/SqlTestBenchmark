using BenchmarkDotNet.Attributes;
using DatabaseEF31;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTestBenchmark31.Tests
{
    public class PerformanceTest31
    {
        private const int NUMBER_OF_RECORDS = 1000; 

        private BenchmarkContext dbContext;
        private string recordIdsList;
        private string[] recordIds;

        [GlobalSetup]
        public void GlobalSetup()
        {
            dbContext = new BenchmarkContext();
            recordIdsList = string.Join(",", Enumerable.Range(1, NUMBER_OF_RECORDS));
            recordIds = recordIdsList.Split(",");
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            //Write your cleanup logic here
            dbContext?.Dispose();
        }

        [Benchmark]
        public List<TestEntity> GetEntitiesByContains()
        {
            var recordIds = recordIdsList.Split(",");
            var data = dbContext.TestEntities
                .Where(e => recordIds.Contains(e.RecordId.ToString()))
                .ToList();

            return data;
        }

        [Benchmark(Baseline = true)]
        public List<TestEntity> GetEntitiesByIn()
        {
            var recordIds = recordIdsList.Split(",");

            var values = new StringBuilder();
            values.AppendFormat("{0}", recordIds[0]);
            for (int i = 1; i < recordIds.Count(); i++)
                values.AppendFormat(", {0}", recordIds[i]);

            var sql = string.Format(
                "SELECT * FROM [dbo].[TestEntities] WHERE [RecordId] IN ({0})",
            values);

            var data = dbContext.TestEntities.FromSqlRaw(sql)
                .ToList();

            return data;
        }

        [Benchmark]
        public List<TestEntity> GetEntitiesByInWithJoinAndSplit()
        {
            var recordIdsLocal = string.Join(",", recordIds).Split(",");

            var values = new StringBuilder();
            values.AppendFormat("{0}", recordIdsLocal[0]);
            for (int i = 1; i < recordIdsLocal.Count(); i++)
                values.AppendFormat(", {0}", recordIdsLocal[i]);

            var sql = string.Format(
                "SELECT * FROM [dbo].[TestEntities] WHERE [RecordId] IN ({0})",
            values);

            var data = dbContext.TestEntities.FromSqlRaw(sql)
                .ToList();

            return data;
        }

        [Benchmark]
        public List<TestEntity> GetEntitiesByInWithoutSplitting()
        {
            var values = new StringBuilder();
            values.AppendFormat("{0}", recordIds[0]);
            for (int i = 1; i < recordIds.Count(); i++)
                values.AppendFormat(", {0}", recordIds[i]);

            var sql = string.Format(
                "SELECT * FROM [dbo].[TestEntities] WHERE [RecordId] IN ({0})",
            values);

            var data = dbContext.TestEntities.FromSqlRaw(sql)
                .ToList();

            return data;
        }
    }
}
