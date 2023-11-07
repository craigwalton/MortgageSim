using CommunityToolkit.Diagnostics;

namespace PropertySim.Csv;

public sealed class Writer : IDisposable
{
    private readonly StreamWriter _writer;
    private readonly int _columns;

    private const string DefaultDataDir = "../../../../../Data";

    private Writer(string dir, string filename, params string[] columnHeaders)
    {
        Guard.IsGreaterThan(columnHeaders.Length, 0);

        _writer = new StreamWriter(Path.Combine(dir, filename));
        _columns = columnHeaders.Length;
        _writer.WriteLine(string.Join(',', columnHeaders));
    }

    public Writer(string filename, params string[] columnHeaders)
        : this(DefaultDataDir, filename, columnHeaders)
    {
    }

    public Writer(DirectoryInfo dir, string filename, params string[] columnHeaders)
        : this(dir.ToString(), filename, columnHeaders)
    {
    }

    public void WriteLine(params object[] values)
    {
        Guard.IsEqualTo(values.Length, _columns);

        _writer.WriteLine(string.Join(',', values));
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
