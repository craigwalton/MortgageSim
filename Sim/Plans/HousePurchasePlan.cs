using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class HousePurchasePlan : Plan
{
    private readonly PropertyValue _propertyValue;
    private readonly InterestRate _mortgageInterestRate;

    public HousePurchasePlan(
        PropertyValue propertyValue,
        decimal deposit,
        int mortgageTermYears,
        InterestRate mortgageInterestRate,
        StreamWriter output)
    {
        _propertyValue = propertyValue;
        _mortgageInterestRate = mortgageInterestRate;
        Mortgage = new VariableMortgage(propertyValue.Value - deposit, mortgageTermYears, output);
    }

    public VariableMortgage Mortgage { get; }

    public void ProcessMonth(out decimal mortgagePayment)
    {
        mortgagePayment = Mortgage.MakePayment(_mortgageInterestRate);
    }

    public override decimal ComputeEquity()
    {
        var liabilities = Mortgage.RemainingLoan;
        var assets = _propertyValue.Value;
        return assets - liabilities;
    }
}
