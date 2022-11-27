using Microsoft.EntityFrameworkCore;

namespace Database;

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
        optionsBuilder
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=testDB;Trusted_Connection=True;Connection Timeout=3600");
        //.LogTo(Console.WriteLine, LogLevel.Information);
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