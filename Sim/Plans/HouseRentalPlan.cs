using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class HouseRentalPlan : Plan
{
    public HouseRentalPlan(decimal deposit, RentPrice rentPrice, InterestRate savingsInterestRate, StreamWriter output)
    {
        Rent = new Rent(rentPrice, output);
        Savings = new Savings(deposit, savingsInterestRate, output);
    }

    public Rent Rent { get; }

    public Savings Savings { get; }

    public void ProcessMonth(decimal amountAvailable, Time time)
    {
        var rentPayment = Rent.TakePayment(time);
        Savings.MakeMonthlyPayment(amountAvailable - rentPayment);
    }

    public override decimal ComputeEquity(Time time)
    {
        return Savings.Balance;
    }
}
