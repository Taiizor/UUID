using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace UUIDBenchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class DatabaseOptimizationBenchmarks
    {
        private const int BatchSize = 10000;
        private UUID[] _uuidArray;

        [GlobalSetup]
        public void Setup()
        {
            _uuidArray = new UUID[BatchSize];
        }

        [Benchmark(Baseline = true)]
        public UUID GenerateRandomV4()
        {
            return UUID.NewV4();
        }

        [Benchmark]
        public UUID GeneratePostgreSQLOptimized()
        {
            return UUID.NewDatabaseOptimized(DatabaseType.PostgreSQL);
        }

        [Benchmark]
        public UUID GenerateSQLServerOptimized()
        {
            return UUID.NewDatabaseOptimized(DatabaseType.SQLServer);
        }

        [Benchmark]
        public void BulkGenerateSequential()
        {
            Toolkit.CreateSequentialBatch(BatchSize);
        }

        [Benchmark]
        public void BulkGenerateRandom()
        {
            _uuidArray.TryFill();
        }

        [Benchmark]
        public void BulkGeneratePostgreSQL()
        {
            Toolkit.CreateSequentialBatch(BatchSize, DatabaseType.PostgreSQL);
        }

        [Benchmark]
        public void BulkGenerateSQLServer()
        {
            Toolkit.CreateSequentialBatch(BatchSize, DatabaseType.SQLServer);
        }
    }
}
