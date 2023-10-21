namespace PropertySim.Plans;

public sealed class HouseRentalPlan : Plan
{
    private readonly decimal _monthlyRent;

    public HouseRentalPlan(decimal deposit, decimal monthlyRent, StreamWriter output)
    {
        _monthlyRent = monthlyRent;
        Savings = new Savings(deposit, output);
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
