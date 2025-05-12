using BenchmarkDotNet.Running;
using effective_csharp.bench;

namespace effective_csharp;

public class Program
{
    public static void Main(string[] args)
    {
        var aa = Directory.GetCurrentDirectory();
        var summaryFileRead = BenchmarkRunner.Run<Bench>();
    }
}