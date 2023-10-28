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
        Mortgage = new VariableMortgage(propertyValue.InitialValue - deposit, mortgageTermYears, mortgageInterestRate, output);
    }

    public VariableMortgage Mortgage { get; }

    public void ProcessMonth(out decimal mortgagePayment)
    {
        mortgagePayment = Mortgage.TakePayment();
    }

    public override decimal ComputeEquity(Time time)
    {
        var liabilities = Mortgage.OutstandingLoan;
        var assets = _propertyValue.ComputeValue(time);
        return assets - liabilities;
    }
}
