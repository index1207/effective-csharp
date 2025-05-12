namespace effective_csharp;

public class File
{
    public File(string path) { _path = path; }
    
    public string? ReadLine()
    {
        using var reader = new StreamReader(_path);
        return reader.ReadLine();
    }

    private readonly string _path;
}

public class DisposableFile : IDisposable
{
    public DisposableFile(string path)
    {
        _reader = new StreamReader(path);
    }
    
    public string? ReadLine() => _reader.ReadLine();
    
    private readonly StreamReader _reader;

    public void Dispose()
    {
        _reader.Dispose();
    }
}