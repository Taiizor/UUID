using BenchmarkDotNet.Running;

namespace UUIDCompareBenchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CompareBenchmarks>();
        }
    }
}