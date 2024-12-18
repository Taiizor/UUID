using BenchmarkDotNet.Running;

namespace UUIDSerializationSystemBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerializationBenchmarks>();
        }
    }
}