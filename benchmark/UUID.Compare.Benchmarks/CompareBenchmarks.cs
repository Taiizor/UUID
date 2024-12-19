using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

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
        public void Generate_MultipleUUIDs()
        {
            for (int i = 0; i < 1000; i++)
            {
                _ = UUID.New();
            }
        }
    }
}