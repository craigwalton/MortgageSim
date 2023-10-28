namespace PropertySim;

public class CsvWriter : IDisposable
{
    private readonly StreamWriter _writer;
    private readonly int _columns;

    public CsvWriter(string filepath, params string[] columnHeaders)
    {
        _writer = new StreamWriter($"../../../../data/{filepath}");
        _columns = columnHeaders.Length;
        _writer.WriteLine(string.Join(',', columnHeaders));
    }

    public void WriteLine(params object[] values)
    {
        if (values.Length != _columns)
        {
            throw new InvalidOperationException(
                $"Number of values ({values.Length}) must be the equal to the number of columns ({_columns}).");
        }
        _writer.WriteLine(string.Join(',', values));
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}