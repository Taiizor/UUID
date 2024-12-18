using BenchmarkDotNet.Running;

namespace UUIDBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<UUIDBenchmarks>();
        }
    }
}