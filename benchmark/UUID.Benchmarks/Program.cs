using BenchmarkDotNet.Running;

namespace UUID.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<UUIDBenchmarks>();
        }
    }
}