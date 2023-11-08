using MortgageSim.Sim.Accounts;
using MortgageSim.Sim.Tests.UnitTesting;
using MortgageSim.Sim.Variables;
using Xunit;

namespace MortgageSim.Sim.Tests.Accounts;

public sealed class FixedMortgageTests
{
    [Fact]
    public void Can_take_monthly_payment()
    {
        var sut = new FixedMortgage(100_000m, 25, new InterestRate(0.01m));

        sut.TakeMonthlyPayment();

        Assert.Equal(99706.46m, sut.OutstandingLoan, precision: 2);
    }

    [Fact]
    public void Taking_final_monthly_payment_results_in_zero_loan()
    {
        var sut = new FixedMortgage(10_000m, 1, new InterestRate(0m));
        Utils.Repeat(() => Assert.Equal(833.3333333333333333333333333m, sut.TakeMonthlyPayment()), 11);

        var actualFinalPayment = sut.TakeMonthlyPayment();

        Assert.Equal(833.3333333333333333333333343m, actualFinalPayment);
        Assert.Equal(0m, sut.OutstandingLoan);
    }

    [Fact]
    public void Taking_monthly_payment_after_final_payment_throws()
    {
        var sut = new FixedMortgage(100_000m, 1, new InterestRate(0.01m));
        Utils.Repeat(() => sut.TakeMonthlyPayment(), 12);

        Assert.Throws<InvalidOperationException>(() => sut.TakeMonthlyPayment());
    }
}
