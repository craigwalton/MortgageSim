using PropertySim.Accounts;
using PropertySim.Variables;
using Sim.Tests.UnitTesting;
using Xunit;

namespace Sim.Tests.Accounts;

public class FixedMortgageTests
{
    [Fact]
    public void Can_take_payment()
    {
        var sut = new FixedMortgage(100_000m, 25, new InterestRate(0.01m), StreamWriter.Null);

        sut.TakePayment();

        Assert.Equal(99706.46m, sut.OutstandingLoan, precision: 2);
    }

    [Fact]
    public void Final_payment_results_in_zero_loan()
    {
        var sut = new FixedMortgage(10_000m, 1, new InterestRate(0m), StreamWriter.Null);
        Utils.Repeat(() => Assert.Equal(833.3333333333333333333333333m, sut.TakePayment()), 11);

        var actualFinalPayment = sut.TakePayment();

        Assert.Equal(833.3333333333333333333333343m, actualFinalPayment);
        Assert.Equal(0m, sut.OutstandingLoan);
    }

    [Fact]
    public void Taking_payment_after_final_payment_throws()
    {
        var sut = new FixedMortgage(100_000m, 1, new InterestRate(0.01m), StreamWriter.Null);
        Utils.Repeat(() => sut.TakePayment(), 12);

        Assert.Throws<InvalidOperationException>(() => sut.TakePayment());
    }
}
