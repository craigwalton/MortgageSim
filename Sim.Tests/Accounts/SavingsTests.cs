using PropertySim.Accounts;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests.Accounts;

public sealed class SavingsTests
{
    [Fact]
    public void Sets_initial_balance()
    {
        var sut = new Savings(100m, new InterestRate(0.01m), StreamWriter.Null);

        var actual = sut.Balance;

        Assert.Equal(100m, actual);
    }

    [Fact]
    public void Can_make_monthly_payment()
    {
        var sut = new Savings(100m, new InterestRate(0.12m), StreamWriter.Null);

        sut.MakeMonthlyPayment(10m);

        Assert.Equal(111m, sut.Balance);
    }

    [Fact]
    public void Can_make_zero_monthly_payment()
    {
        var sut = new Savings(100m, new InterestRate(0.12m), StreamWriter.Null);

        sut.MakeMonthlyPayment(0m);
        sut.MakeMonthlyPayment(0m);
        sut.MakeMonthlyPayment(0m);

        Assert.Equal(103.0301m, sut.Balance);
    }

    [Fact]
    public void Interest_compounds()
    {
        var sut = new Savings(100m, new InterestRate(0.12m), StreamWriter.Null);

        sut.MakeMonthlyPayment(10m);
        sut.MakeMonthlyPayment(10m);

        Assert.Equal(122.11m, sut.Balance);
    }

    [Fact]
    public void Can_open_empty_account()
    {
        var sut = new Savings(0m, new InterestRate(0.12m), StreamWriter.Null);

        sut.MakeMonthlyPayment(0m);

        Assert.Equal(0m, sut.Balance);
    }

    [Fact]
    public void Supports_zero_interest_rate()
    {
        var sut = new Savings(100m, new InterestRate(0m), StreamWriter.Null);

        sut.MakeMonthlyPayment(0m);

        Assert.Equal(100m, sut.Balance);
    }
}
