using MortgageSim.Sim.Variables;
using Xunit;

namespace MortgageSim.Sim.Tests.Variables;

public sealed class RentPriceTests
{
    [Fact]
    public void Can_create()
    {
        var sut = new RentPrice(700m, 0.01m);

        Assert.Equal(700m, sut.InitialMonthly);
        Assert.Equal(0.01m, sut.YearlyIncrease);
        Assert.Equal(700m, sut.ComputeMonthlyPrice(new Time()));
    }

    [Fact]
    public void Can_compute_current_value()
    {
        var sut = new RentPrice(700m, 0.01m);

        var actual = sut.ComputeMonthlyPrice(new Time(month: 3, year: 2));

        Assert.Equal(714.07m, actual);
    }

    [Fact]
    public void Can_compute_current_value_with_constant_yearly_value()
    {
        var sut = new RentPrice(700m, 0m);

        var actual = sut.ComputeMonthlyPrice(new Time(month: 7, year: 2));

        Assert.Equal(700m, actual);
    }

    [Fact]
    public void Can_compute_current_value_with_decreasing_yearly_value()
    {
        var sut = new RentPrice(700m, -0.01m);

        var actual = sut.ComputeMonthlyPrice(new Time(month: 11, year: 2));

        Assert.Equal(686.07m, actual);
    }
}
