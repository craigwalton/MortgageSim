using PropertySim;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests.Variables;

public sealed class PropertyValueTests
{
    [Fact]
    public void Can_create()
    {
        var sut = new PropertyValue(100_000m, 0.01m);

        Assert.Equal(100_000m, sut.InitialValue);
        Assert.Equal(0.01m, sut.YearlyIncrease);
        Assert.Equal(100_000m, sut.ComputeValue(new Time()));
    }

    [Fact]
    public void Can_compute_current_value()
    {
        var sut = new PropertyValue(100_000m, 0.01m);

        var actual = sut.ComputeValue(new Time(month: 3, year: 2));

        Assert.Equal(102_010m, actual);
    }

    [Fact]
    public void Can_compute_current_value_with_decreasing_yearly_value()
    {
        var sut = new PropertyValue(100_000m, -0.01m);

        var actual = sut.ComputeValue(new Time(month: 7, year: 2));

        Assert.Equal(98_010m, actual);
    }
}
