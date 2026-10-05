using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UUIDSerializationEntityBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net11_0, launchCount: 1, warmupCount: 2, iterationCount: 3)]
    public class UUIDConverterBenchmarks : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly BenchmarkDbContext _context;
        private readonly UUID _uuid;

        public UUIDConverterBenchmarks()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            DbContextOptions<BenchmarkDbContext> options = new DbContextOptionsBuilder<BenchmarkDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new BenchmarkDbContext(options);
            _context.Database.EnsureCreated();

            _uuid = UUID.New();
        }

        [Benchmark(Baseline = true)]
        public async Task SaveAndLoadUUIDAsBytes()
        {
            BenchmarkEntity entity = new() { ByteUUID = _uuid };

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            BenchmarkEntity loadedEntity = await _context.Entities.FirstAsync();

            if (loadedEntity.ByteUUID != _uuid)
            {
                throw new Exception("UUID mismatch");
            }
        }

        [Benchmark]
        public async Task SaveAndLoadUUIDAsString()
        {
            BenchmarkEntity entity = new() { StringUUID = _uuid };

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            BenchmarkEntity loadedEntity = await _context.Entities.FirstAsync();

            if (loadedEntity.StringUUID != _uuid)
            {
                throw new Exception("UUID mismatch");
            }
        }

        [Benchmark]
        public async Task SaveAndLoadUUIDAsBase64()
        {
            BenchmarkEntity entity = new() { Base64UUID = _uuid };

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            BenchmarkEntity loadedEntity = await _context.Entities.FirstAsync();

            if (loadedEntity.Base64UUID != _uuid)
            {
                throw new Exception("UUID mismatch");
            }
        }

        [Benchmark]
        public async Task SaveAndLoadAllUUIDTypes()
        {
            BenchmarkEntity entity = new()
            {
                ByteUUID = _uuid,
                StringUUID = _uuid,
                Base64UUID = _uuid
            };

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            BenchmarkEntity loadedEntity = await _context.Entities.FirstAsync();

            if (loadedEntity.ByteUUID != _uuid ||
                loadedEntity.StringUUID != _uuid ||
                loadedEntity.Base64UUID != _uuid)
            {
                throw new Exception("UUID mismatch");
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}