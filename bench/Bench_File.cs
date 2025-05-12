using BenchmarkDotNet.Attributes;

namespace effective_csharp.bench;

[MemoryDiagnoser] // 메모리 측정 활성화
public partial class Bench
{
    [Benchmark]
    public void FileRead()
    {
        var file = new effective_csharp.File("/Users/dev.index/Projects/effective-csharp/image/CastT.png");
        file.ReadLine();
    }

    [Benchmark]
    public void DisposableFileRead()
    {
        var file = new DisposableFile("/Users/dev.index/Projects/effective-csharp/image/CastT.png");
        file.ReadLine();
    }
}