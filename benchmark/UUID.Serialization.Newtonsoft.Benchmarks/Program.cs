using BenchmarkDotNet.Running;

namespace UUIDSerializationNewtonsoftBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SerializationBenchmarks>();
        }
    }
}