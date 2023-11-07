using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

internal sealed class RentalPlan
{
    private readonly Rent _rent;
    private readonly Savings _savings;

    public RentalPlan(decimal deposit, RentPrice rentPrice, InterestRate savingsInterestRate)
    {
        _rent = new Rent(rentPrice);
        _savings = new Savings(deposit, savingsInterestRate);
    }

    public void ProcessMonth(decimal amountAvailable, Time time)
    {
        var rentPayment = _rent.TakeMonthlyPayment(time);
        _savings.MakeMonthlyPayment(amountAvailable - rentPayment);
    }

    public decimal ComputeEquity()
    {
        return _savings.Balance;
    }
}
