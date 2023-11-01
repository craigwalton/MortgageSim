using PropertySim;
using PropertySim.Accounts;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests.Accounts;

public class RentTests
{
    [Fact]
    public void Can_take_payment()
    {
        var sut = new Rent(new RentPrice(100m, 0.01m), StreamWriter.Null);

        var actual = sut.TakePayment(new Time());

        Assert.Equal(100m, actual);
    }

    [Fact]
    public void Computes_yearly_rent_increase()
    {
        var sut = new Rent(new RentPrice(100m, 0.01m), StreamWriter.Null);

        Assert.Equal(100m, sut.TakePayment(new Time(month: 0, year: 0)));
        Assert.Equal(100m, sut.TakePayment(new Time(month: 1, year: 0)));
        Assert.Equal(101m, sut.TakePayment(new Time(month: 0, year: 1)));
        Assert.Equal(102.01m, sut.TakePayment(new Time(month: 0, year: 2)));
    }
}
