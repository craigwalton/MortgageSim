namespace PropertySim.Plans;

public sealed class HousePurchasePlan : Plan
{
    private readonly decimal _propertyValue;

    public HousePurchasePlan(decimal propertyValue, decimal deposit, int mortgageTermYears, StreamWriter output)
    {
        _propertyValue = propertyValue;
        Mortgage = new VariableMortgage(propertyValue - deposit, mortgageTermYears, output);
        Savings = new Savings(0m, output);
    }

    public VariableMortgage Mortgage { get; }

    public Savings Savings { get; }

    public override void ProcessMonth(
        decimal income,
        InterestRate mortgageInterestRate,
        InterestRate savingsInterestRate)
    {
        var mortgagePayment = Mortgage.MakePayment(mortgageInterestRate);
        var incomeForSavings = income - mortgagePayment;
        Savings.MakePayment(incomeForSavings, savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        var liabilities = Mortgage.RemainingLoan;
        var assets = _propertyValue + Savings.Balance;
        return assets - liabilities;
    }
}
