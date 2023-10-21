using MathNet.Numerics.Distributions;

namespace PropertySim.Plans;

public sealed class HousePurchasePlan : Plan
{
    private readonly PropertyValue _propertyValue;
    private readonly InterestRate _mortgageInterestRate;
    private readonly InterestRate _savingsInterestRate;

    public HousePurchasePlan(
        PropertyValue propertyValue,
        decimal deposit,
        int mortgageTermYears,
        InterestRate mortgageInterestRate,
        InterestRate savingsInterestRate,
        StreamWriter output)
    {
        _propertyValue = propertyValue;
        _mortgageInterestRate = mortgageInterestRate;
        _savingsInterestRate = savingsInterestRate;
        Mortgage = new VariableMortgage(propertyValue.Value - deposit, mortgageTermYears, output);
        Savings = new Savings(0m, output);
    }

    public VariableMortgage Mortgage { get; }

    public Savings Savings { get; }

    public override void ProcessMonth(decimal income)
    {
        var mortgagePayment = Mortgage.MakePayment(_mortgageInterestRate);
        var incomeForSavings = income - mortgagePayment;
        Savings.MakePayment(incomeForSavings, _savingsInterestRate);
    }

    public override decimal ComputeEquity()
    {
        var liabilities = Mortgage.RemainingLoan;
        var assets = _propertyValue.Value + Savings.Balance;
        return assets - liabilities;
    }
}
