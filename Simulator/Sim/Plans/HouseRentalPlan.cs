using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class HouseRentalPlan
{
    public HouseRentalPlan(decimal deposit, RentPrice rentPrice, InterestRate savingsInterestRate)
    {
        Rent = new Rent(rentPrice);
        Savings = new Savings(deposit, savingsInterestRate);
    }

    public Rent Rent { get; }

    public Savings Savings { get; }

    public void ProcessMonth(decimal amountAvailable, Time time)
    {
        var rentPayment = Rent.TakeMonthlyPayment(time);
        Savings.MakeMonthlyPayment(amountAvailable - rentPayment);
    }

    public decimal ComputeEquity(Time time)
    {
        return Savings.Balance;
    }
}
