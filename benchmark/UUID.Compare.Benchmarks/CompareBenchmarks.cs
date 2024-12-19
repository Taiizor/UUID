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
    [SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 2, iterationCount: 3)]
    public class CompareBenchmarks
    {
        [Benchmark(Baseline = true)]
        public Guid Generate_NewGuid()
        {
            return Guid.NewGuid();
        }

        [Benchmark]
        public Guid Generate_NewGuidV7()
        {
            return Guid.CreateVersion7();
        }

        [Benchmark]
        public long Generate_NewIdGen()
        {
            IdGenerator generator = new(0);
            return generator.CreateId();
        }

        [Benchmark]
        public Cuid2 Generate_NewCuid2()
        {
            return new();
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
        public void Generate_MultipleGuids()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Guid.NewGuid();
            }
        }

        [Benchmark]
        public void Generate_MultipleGuidsV7()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Guid.CreateVersion7();
            }
        }

        [Benchmark]
        public void Generate_MultipleIdGens()
        {
            for (int i = 0; i < 1000; i++)
            {
                IdGenerator generator = new(0);
                _ = generator.CreateId();
            }
        }

        [Benchmark]
        public void Generate_MultipleCuid2s()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = new Cuid2();
            }
        }

        [Benchmark]
        public void Generate_MultipleCuids()
        {
            for (int i = 0; i < 1000; i++)
            {
                Cuid.NewCuid();
            }
        }

        [Benchmark]
        public void Generate_MultipleUlid()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = Ulid.NewUlid();
            }
        }

        [Benchmark]
        public void Generate_MultipleUUIDs()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = UUID.New();
            }
        }
    }
}