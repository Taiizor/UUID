using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using IdGen;
using Visus.Cuid;

namespace UUIDCompareBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net11_0, launchCount: 1, warmupCount: 2, iterationCount: 3)]
    public class CompareBenchmarks
    {
        [Benchmark(Baseline = true)]
        public Guid Generate_NewGuidV7()
        {
            return Guid.CreateVersion7();
        }

        [Benchmark]
        public Cuid2 Generate_NewCuid2()
        {
            return new();
        }

        [Benchmark]
        public long Generate_NewIdGen()
        {
            IdGenerator generator = new(0);
            return generator.CreateId();
        }

        [Benchmark]
        public Guid Generate_NewGuid()
        {
            return Guid.NewGuid();
        }

        [Benchmark]
        public Cuid Generate_NewCuid()
        {
            return Cuid.NewCuid();
        }

        [Benchmark]
        public Ulid Generate_NewUlid()
        {
            return Ulid.NewUlid();
        }

        [Benchmark]
        public UUID Generate_NewUUID()
        {
            return UUID.New();
        }

        [Benchmark]
        public void Generate_MultipleNewGuidsV7()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Guid.CreateVersion7();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewIdGens()
        {
            for (int i = 0; i < 1000; i++)
            {
                IdGenerator generator = new(0);
                _ = generator.CreateId();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewCuid2s()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = new Cuid2();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewGuids()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Guid.NewGuid();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewCuids()
        {
            for (int i = 0; i < 1000; i++)
            {
                Cuid.NewCuid();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewUlids()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Ulid.NewUlid();
            }
        }

        [Benchmark]
        public void Generate_MultipleNewUUIDs()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = UUID.New();
            }
        }
    }
}