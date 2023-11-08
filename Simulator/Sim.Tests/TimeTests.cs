using MortgageSim.Sim.Tests.UnitTesting;
using Xunit;

namespace MortgageSim.Sim.Tests;

public sealed class TimeTests
{
    [Fact]
    public void Time_starts_at_zero()
    {
        var sut = new Time();

        Assert.Equal(0, sut.Month);
        Assert.Equal(0, sut.Year);
    }

    [Fact]
    public void Can_advance_time()
    {
        var sut = new Time();

        sut.AdvanceOneMonth();

        Assert.Equal(1, sut.Month);
        Assert.Equal(0, sut.Year);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    [InlineData(11, 11, 0)]
    [InlineData(12, 0, 1)]
    [InlineData(13, 1, 1)]
    [InlineData(24, 0, 2)]
    public void Can_advance_multiple_times(int advances, int expectedMonth, int expectedYear)
    {
        var sut = new Time();

        Utils.Repeat(() => sut.AdvanceOneMonth(), advances);

        Assert.Equal(expectedMonth, sut.Month);
        Assert.Equal(expectedYear, sut.Year);
    }
}
