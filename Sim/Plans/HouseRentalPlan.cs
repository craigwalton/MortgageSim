using PropertySim.Accounts;

namespace PropertySim.Plans;

public sealed class HouseRentalPlan : Plan
{
    private readonly RentPrice _rent;
    private readonly InterestRate _savingsInterestRate;

    public HouseRentalPlan(decimal deposit, RentPrice rent, InterestRate savingsInterestRate, StreamWriter output)
    {
        _rent = rent;
        _savingsInterestRate = savingsInterestRate;
        Rent = new Rent(output);
        Savings = new Savings(deposit, output);
    }

    public Rent Rent { get; }

    public Savings Savings { get; }

    public void ProcessMonth(decimal amountAvailable)
    {
        Rent.MakePayment(_rent.MonthlyPrice);
        Savings.MakePayment(amountAvailable - _rent.MonthlyPrice, _savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        return Savings.Balance;
    }
}
