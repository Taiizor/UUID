using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using MessagePack;
using MessagePack.Resolvers;

namespace UUIDSerializationMessagePackBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net11_0, launchCount: 1, warmupCount: 2, iterationCount: 3)]
    public class UUIDFormatterBenchmarks
    {
        private readonly MessagePackSerializerOptions _options;
        private readonly UUID _uuid;
        private readonly byte[] _serializedUuid;
        private readonly List<UUID> _uuidList;
        private readonly byte[] _serializedUuidList;
        private readonly Dictionary<UUID, string> _uuidDictionary;
        private readonly byte[] _serializedUuidDictionary;
        private readonly TestModel _model;
        private readonly byte[] _serializedModel;

        public UUIDFormatterBenchmarks()
        {
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(CompositeResolver.Create(
                    UUIDResolver.Instance,
                    StandardResolver.Instance
                ));

            _uuid = new UUID();
            _serializedUuid = MessagePackSerializer.Serialize(_uuid, _options);

            _uuidList = [new(), new(), new()];
            _serializedUuidList = MessagePackSerializer.Serialize(_uuidList, _options);

            _uuidDictionary = new Dictionary<UUID, string>
            {
                { new UUID(), "Value1" },
                { new UUID(), "Value2" },
                { new UUID(), "Value3" }
            };
            _serializedUuidDictionary = MessagePackSerializer.Serialize(_uuidDictionary, _options);

            _model = new TestModel
            {
                Id = new UUID(),
                Name = "Test",
                Items = [new(), new()]
            };
            _serializedModel = MessagePackSerializer.Serialize(_model, _options);
        }

        [Benchmark]
        public byte[] Serialize_SingleUUID()
        {
            return MessagePackSerializer.Serialize(_uuid, _options);
        }

        [Benchmark]
        public UUID Deserialize_SingleUUID()
        {
            return MessagePackSerializer.Deserialize<UUID>(_serializedUuid, _options);
        }

        [Benchmark]
        public byte[] Serialize_UUIDList()
        {
            return MessagePackSerializer.Serialize(_uuidList, _options);
        }

        [Benchmark]
        public List<UUID> Deserialize_UUIDList()
        {
            return MessagePackSerializer.Deserialize<List<UUID>>(_serializedUuidList, _options);
        }

        [Benchmark]
        public byte[] Serialize_UUIDDictionary()
        {
            return MessagePackSerializer.Serialize(_uuidDictionary, _options);
        }

        [Benchmark]
        public Dictionary<UUID, string> Deserialize_UUIDDictionary()
        {
            return MessagePackSerializer.Deserialize<Dictionary<UUID, string>>(_serializedUuidDictionary, _options);
        }

        [Benchmark]
        public byte[] Serialize_ComplexModel()
        {
            return MessagePackSerializer.Serialize(_model, _options);
        }

        [Benchmark]
        public TestModel Deserialize_ComplexModel()
        {
            return MessagePackSerializer.Deserialize<TestModel>(_serializedModel, _options);
        }
    }

    [MessagePackObject]
    public class TestModel
    {
        [Key(0)]
        public UUID Id { get; set; }

        [Key(1)]
        public string Name { get; set; } = "";

        [Key(2)]
        public List<UUID> Items { get; set; } = [];
    }
}