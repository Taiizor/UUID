using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Dapper;
using Microsoft.Data.SqlClient;

namespace UUIDSerializationDapperBenchmarks
{
    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net11_0, launchCount: 1, warmupCount: 2, iterationCount: 3)]
    public class UUIDHandlerBenchmarks
    {
        private readonly UUID _uuid;
        private readonly string _base64Value;
        private readonly byte[] _binaryValue;
        private readonly string _stringValue;
        private readonly SqlParameter _parameter;
        private readonly Base64UUIDHandler _base64Handler;
        private readonly BinaryUUIDHandler _binaryHandler;
        private readonly StringUUIDHandler _stringHandler;

        public UUIDHandlerBenchmarks()
        {
            _uuid = UUID.New();
            _base64Value = _uuid.ToBase64();
            _stringValue = _uuid.ToString();
            _binaryValue = _uuid.ToByteArray();
            _parameter = new SqlParameter();
            _base64Handler = new Base64UUIDHandler();
            _binaryHandler = new BinaryUUIDHandler();
            _stringHandler = new StringUUIDHandler();

            SqlMapper.AddTypeHandler(_base64Handler);
            SqlMapper.AddTypeHandler(_binaryHandler);
            SqlMapper.AddTypeHandler(_stringHandler);
        }

        [Benchmark(Baseline = true)]
        public void Base64_SetValue()
        {
            _base64Handler.SetValue(_parameter, _uuid);
        }

        [Benchmark]
        public void Base64_Parse()
        {
            _ = _base64Handler.Parse(_base64Value);
        }

        [Benchmark]
        public void Binary_SetValue()
        {
            _binaryHandler.SetValue(_parameter, _uuid);
        }

        [Benchmark]
        public void Binary_Parse()
        {
            _ = _binaryHandler.Parse(_binaryValue);
        }

        [Benchmark]
        public void String_SetValue()
        {
            _stringHandler.SetValue(_parameter, _uuid);
        }

        [Benchmark]
        public void String_Parse()
        {
            _ = _stringHandler.Parse(_stringValue);
        }

        [Benchmark]
        public void Base64_SetValue_Empty()
        {
            _base64Handler.SetValue(_parameter, UUID.Empty);
        }

        [Benchmark]
        public void Binary_SetValue_Empty()
        {
            _binaryHandler.SetValue(_parameter, UUID.Empty);
        }

        [Benchmark]
        public void String_SetValue_Empty()
        {
            _stringHandler.SetValue(_parameter, UUID.Empty);
        }
    }
}