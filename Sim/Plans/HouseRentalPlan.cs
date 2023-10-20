namespace PropertySim.Plans;

public sealed class HouseRentalPlan : Plan
{
    private readonly decimal _monthlyRent;

    public HouseRentalPlan(decimal deposit, decimal monthlyRent)
    {
        _monthlyRent = monthlyRent;
        Savings = new Savings(deposit);
    }

    public Savings Savings { get; }

    public override void ProcessMonth(decimal income, InterestRate _, InterestRate savingsInterestRate)
    {
        var incomeForSavings = income - _monthlyRent;
        Savings.MakePayment(incomeForSavings, savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        return Savings.Balance;
    }
}
