using System.Reflection;
using CommunityToolkit.Diagnostics;

namespace MortgageSim.Sim.Csv;

internal sealed class Writer : IDisposable
{
    private readonly StreamWriter _writer;
    private readonly int _columns;

    private Writer(string writerDir, string writerFilename, params string[] columnHeaders)
    {
        Guard.IsGreaterThan(columnHeaders.Length, 0);

        _writer = new StreamWriter(Path.Combine(writerDir, writerFilename));
        _columns = columnHeaders.Length;
        _writer.WriteLine(string.Join(',', columnHeaders));
    }

    public static Writer Create(string filename, params string[] columnHeaders)
    {
        return new Writer(GetDefaultResultsDir(), filename, columnHeaders);
    }

    public static Writer Create(DirectoryInfo dir, string filename, params string[] columnHeaders)
    {
        return new Writer(dir.ToString(), filename, columnHeaders);
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

    private static string GetDefaultResultsDir()
    {
        // The Results directory is in the repo root. Use the assembly path to be agnostic to CWD.
        var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
        var repoRoot = assemblyDirectory?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
        if (repoRoot == null)
        {
            throw new DirectoryNotFoundException(
                $"Failed to find repo root dir relative to assembly directory: '{assemblyDirectory}'.");
        }
        return Path.Combine(repoRoot, "Results");
    }
}
