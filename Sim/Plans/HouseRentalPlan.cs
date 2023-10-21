namespace PropertySim.Plans;

public sealed class HouseRentalPlan : Plan
{
    private readonly RentPrice _rent;

    public HouseRentalPlan(decimal deposit, RentPrice rent, StreamWriter output)
    {
        _rent = rent;
        Savings = new Savings(deposit, output);
    }

    public Savings Savings { get; }

    public override void ProcessMonth(decimal income, InterestRate _, InterestRate savingsInterestRate)
    {
        var incomeForSavings = income - _rent.MonthlyPrice;
        Savings.MakePayment(incomeForSavings, savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        return Savings.Balance;
    }
}
