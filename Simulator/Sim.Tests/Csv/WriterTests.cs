using PropertySim.Csv;
using Xunit;

namespace Sim.Tests.Csv;

public sealed class WriterTests
{
    [Fact]
    public void Can_write_csv()
    {
        var sut = new Writer(new DirectoryInfo("."), "out.csv", "Column a", "Column b");

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
    public void Verifies_number_of_columns()
    {
        var sut = new Writer(new DirectoryInfo("."), "out.csv", "Column a", "Column b");

        Assert.Throws<ArgumentException>(() => sut.WriteLine());
        Assert.Throws<ArgumentException>(() => sut.WriteLine(1));
        Assert.Throws<ArgumentException>(() => sut.WriteLine(1, 2, 3));
    }

    [Fact]
    public void Verifies_at_least_1_column()
    {
        Assert.Throws<ArgumentException>(() => new Writer("out.csv"));
    }
}
