
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.Extensions.Logging.Console;

namespace DatabaseEF31
{

    public class BenchmarkContext : DbContext
    {
        public BenchmarkContext() : base()
        {
        }

        public BenchmarkContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=testDB;Trusted_Connection=True;Connection Timeout=3600")
                .UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestEntity>(builder =>
            {
                builder.ToTable("TestEntities");

                var data = Enumerable.Range(1, 70000)
                    .Select(v => new TestEntity
                    {
                        Id = v,
                        RecordId = v,
                        Name = $"Name{v}"
                    }).ToList();

                builder.HasData(data);

            });
        }
    }
}
