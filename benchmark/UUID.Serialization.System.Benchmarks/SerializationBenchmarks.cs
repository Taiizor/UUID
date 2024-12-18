using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text.Json;

namespace UUIDSerializationSystemBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 3, iterationCount: 5)]
    public class SerializationBenchmarks
    {
        private readonly JsonSerializerOptions _options;
        private readonly UUID _uuid = UUID.New();
        private readonly string _serializedUuid;

        public SerializationBenchmarks()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new UUIDConverter());
            _serializedUuid = JsonSerializer.Serialize(_uuid, _options);
        }

        [Benchmark]
        public string Serialize()
        {
            return JsonSerializer.Serialize(_uuid, _options);
        }

        [Benchmark]
        public UUID Deserialize()
        {
            return JsonSerializer.Deserialize<UUID>(_serializedUuid, _options);
        }

        [Benchmark]
        public string SerializeArray()
        {
            UUID[] array = new[] { _uuid, _uuid, _uuid };
            return JsonSerializer.Serialize(array, _options);
        }

        [Benchmark]
        public UUID[] DeserializeArray()
        {
            string serializedArray = JsonSerializer.Serialize(new[] { _uuid, _uuid, _uuid }, _options);
            return JsonSerializer.Deserialize<UUID[]>(serializedArray, _options);
        }
    }
}