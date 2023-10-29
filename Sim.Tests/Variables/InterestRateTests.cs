using PropertySim.Variables;
using Xunit;

namespace Sim.Tests.Variables;

public class InterestRateTests
{
    [Fact]
    public void Can_create_positive()
    {
        var sut = new InterestRate(0.03m);

        Assert.Equal(0.03m, sut.Yearly);
        Assert.Equal(0.0025m, sut.Monthly);
    }

    [Fact]
    public void Can_create_zero()
    {
        var sut = new InterestRate(0m);

        Assert.Equal(0m, sut.Yearly);
        Assert.Equal(0m, sut.Monthly);
    }

    [Fact]
    public void Can_create_negative()
    {
        var sut = new InterestRate(-0.06m);

        Assert.Equal(-0.06m, sut.Yearly);
        Assert.Equal(-0.005m, sut.Monthly);
    }
}
