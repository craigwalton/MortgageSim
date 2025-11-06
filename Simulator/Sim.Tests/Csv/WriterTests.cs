using MortgageSim.Sim.Csv;
using Xunit;

namespace MortgageSim.Sim.Tests.Csv;

public sealed class WriterTests
{
    [Fact]
    public void Can_write_csv()
    {
        var sut = Writer.Create(new DirectoryInfo("."), "out.csv", "Column a", "Column b");

        sut.WriteLine(1, 2.3m);

        sut.Dispose();
        var actual = File.ReadAllLines("out.csv");
        var expected = new[]
        {
            "Column a,Column b",
            "1,2.3"
        };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_get_file_path()
    {
        using var sut = Writer.Create(new DirectoryInfo("."), "out.csv", "Column a", "Column b");

        var actual = sut.FilePath;

        Assert.Equal("./out.csv", actual);
    }

    [Fact]
    public void Verifies_number_of_columns()
    {
        var sut = Writer.Create(new DirectoryInfo("."), "out.csv", "Column a", "Column b");

        Assert.Throws<ArgumentException>(() => sut.WriteLine());
        Assert.Throws<ArgumentException>(() => sut.WriteLine(1));
        Assert.Throws<ArgumentException>(() => sut.WriteLine(1, 2, 3));
    }

    [Fact]
    public void Verifies_at_least_1_column()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Writer.Create("out.csv"));
    }

    [Fact]
    public void Can_create_writer_with_default_results_dir()
    {
        const string expectedCsv = "../../../../../Results/test-file.csv";

        var sut = Writer.Create("test-file.csv", "Column a", "Column b");

        sut.Dispose();
        Assert.True(File.Exists(expectedCsv));
        File.Delete(expectedCsv);
    }
}
