using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class HousePurchasePlan : Plan
{
    private readonly PropertyValue _propertyValue;

    public HousePurchasePlan(
        PropertyValue propertyValue,
        decimal deposit,
        int mortgageTermYears,
        InterestRate mortgageInterestRate,
        StreamWriter output)
    {
        _propertyValue = propertyValue;
        Mortgage = new VariableMortgage(propertyValue.Value - deposit, mortgageTermYears, mortgageInterestRate, output);
    }

    public VariableMortgage Mortgage { get; }

    public void ProcessMonth(out decimal mortgagePayment)
    {
        mortgagePayment = Mortgage.TakePayment();
    }

    public override decimal ComputeEquity()
    {
        var liabilities = Mortgage.OutstandingLoan;
        var assets = _propertyValue.Value;
        return assets - liabilities;
    }
}
