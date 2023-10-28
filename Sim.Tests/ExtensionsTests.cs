using PropertySim;
using Xunit;

namespace Sim.Tests;

public class ExtensionsTests
{
    [Theory]
    [InlineData(2, 0, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(2, 2, 4)]
    [InlineData(2, 8, 256)]
    [InlineData(0, 0, 1)]
    [InlineData(0, 99, 0)]
    public void Test_raise_to_power_of(decimal value, int power, decimal expected)
    {
        var actual = value.RaiseToPowerOf(power);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Raise_to_power_of_throws_for_negative_power()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 5m.RaiseToPowerOf(-1));
    }
}
