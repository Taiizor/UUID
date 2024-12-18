using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace UUIDBenchmarks
{
    [SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 3, iterationCount: 5)]
    [MemoryDiagnoser]
    public class UUIDBenchmarks
    {
        private readonly byte[] _buffer = new byte[16];
        private readonly Guid _guid = Guid.NewGuid();
        private readonly UUID _uuid2 = UUID.New();
        private readonly UUID _uuid = UUID.New();
        private readonly string _uuidString;

        public UUIDBenchmarks()
        {
            _uuidString = _uuid.ToString();
        }

        [Benchmark(Baseline = true)]
        public UUID Generate_New()
        {
            return UUID.New();
        }

        [Benchmark]
        public string Convert_ToString()
        {
            return _uuid.ToString();
        }

        [Benchmark]
        public UUID Parse_FromString()
        {
            return UUID.Parse(_uuidString);
        }

        [Benchmark]
        public bool TryParse_FromString()
        {
            return UUID.TryParse(_uuidString, out _);
        }

        [Benchmark]
        public string Convert_ToBase32()
        {
            return _uuid.ToBase32();
        }

        [Benchmark]
        public string Convert_ToBase64()
        {
            return _uuid.ToBase64();
        }

        [Benchmark]
        public byte[] Convert_ToByteArray()
        {
            return _uuid.ToByteArray();
        }

        [Benchmark]
        public bool Convert_TryWriteBytes()
        {
            return _uuid.TryWriteBytes(_buffer);
        }

        [Benchmark]
        public bool Convert_TryWriteStringify()
        {
            char[] buffer = new char[32];
            return _uuid.TryWriteStringify(buffer);
        }

        [Benchmark]
        public Guid Convert_ToGuid()
        {
            return _uuid.ToGuid();
        }

        [Benchmark]
        public UUID Convert_FromGuid()
        {
            return UUID.FromGuid(_guid);
        }

        [Benchmark]
        public bool Compare_Equals()
        {
            return _uuid.Equals(_uuid2);
        }

        [Benchmark]
        public int Compare_CompareTo()
        {
            return _uuid.CompareTo(_uuid2);
        }

        [Benchmark]
        public bool Compare_LessThan()
        {
            return _uuid < _uuid2;
        }

        [Benchmark]
        public bool Compare_GreaterThan()
        {
            return _uuid > _uuid2;
        }
    }
}