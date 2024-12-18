using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Newtonsoft.Json;

namespace UUIDSerializationNewtonsoftBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 3, iterationCount: 5)]
    public class SerializationBenchmarks
    {
        private readonly JsonSerializerSettings _settings;
        private readonly UUID _uuid = UUID.New();
        private readonly string _serializedUuid;

        public SerializationBenchmarks()
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new UUIDConverter());
            _serializedUuid = JsonConvert.SerializeObject(_uuid, _settings);
        }

        [Benchmark]
        public string Serialize()
        {
            return JsonConvert.SerializeObject(_uuid, _settings);
        }

        [Benchmark]
        public UUID Deserialize()
        {
            return JsonConvert.DeserializeObject<UUID>(_serializedUuid, _settings);
        }

        [Benchmark]
        public string SerializeArray()
        {
            UUID[] array = new[] { _uuid, _uuid, _uuid };
            return JsonConvert.SerializeObject(array, _settings);
        }

        [Benchmark]
        public UUID[] DeserializeArray()
        {
            string serializedArray = JsonConvert.SerializeObject(new[] { _uuid, _uuid, _uuid }, _settings);
            return JsonConvert.DeserializeObject<UUID[]>(serializedArray, _settings);
        }
    }
}