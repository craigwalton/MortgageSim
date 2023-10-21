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
        Savings = new Savings(deposit, output);
    }

    public Savings Savings { get; }

    public override void ProcessMonth(decimal income)
    {
        var incomeForSavings = income - _rent.MonthlyPrice;
        Savings.MakePayment(incomeForSavings, _savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        return Savings.Balance;
    }
}
