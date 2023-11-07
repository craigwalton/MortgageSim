using PropertySim.Accounts;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests.Accounts;

public sealed class SavingsTests
{
    [Fact]
    public void Sets_initial_balance()
    {
        var sut = new Savings(100m, new InterestRate(0.01m));

        var actual = sut.Balance;

        Assert.Equal(100m, actual);
    }

    [Fact]
    public void Can_make_monthly_payment()
    {
        var sut = new Savings(100m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(10m);

        Assert.Equal(111m, sut.Balance);
    }

    [Fact]
    public void Can_make_zero_monthly_payment()
    {
        var sut = new Savings(100m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(0m);
        sut.MakeMonthlyPayment(0m);
        sut.MakeMonthlyPayment(0m);

        Assert.Equal(103.0301m, sut.Balance);
    }

    [Fact]
    public void Interest_compounds()
    {
        var sut = new Savings(100m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(10m);
        sut.MakeMonthlyPayment(10m);

        Assert.Equal(122.11m, sut.Balance);
    }

    [Fact]
    public void Can_open_empty_account()
    {
        var sut = new Savings(0m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(0m);

        Assert.Equal(0m, sut.Balance);
    }

    [Fact]
    public void Supports_zero_interest_rate()
    {
        var sut = new Savings(100m, new InterestRate(0m));

        sut.MakeMonthlyPayment(0m);

        Assert.Equal(100m, sut.Balance);
    }

    [Fact]
    public void Can_make_withdrawal()
    {
        var sut = new Savings(100m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(-50m);

        Assert.Equal(51m, sut.Balance);
    }

    [Fact]
    public void Can_go_into_overdraft()
    {
        var sut = new Savings(100m, new InterestRate(0.12m));

        sut.MakeMonthlyPayment(-200m);

        Assert.Equal(-99m, sut.Balance);
    }
}
