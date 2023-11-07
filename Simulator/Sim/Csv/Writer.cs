namespace PropertySim.Csv;

public sealed class Writer : IDisposable
{
    private readonly StreamWriter _writer;
    private readonly int _columns;

    private const string DefaultDataDir = "../../../../../Data";

    private Writer(string dir, string filename, params string[] columnHeaders)
    {
        if (columnHeaders.Length == 0)
        {
            throw new ArgumentException("Must be at least 1 column header.", nameof(columnHeaders));
        }
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
        if (values.Length != _columns)
        {
            throw new ArgumentException(
                $"Number of values ({values.Length}) must be the equal to the number of columns ({_columns}).",
                nameof(values));
        }
        _writer.WriteLine(string.Join(',', values));
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
