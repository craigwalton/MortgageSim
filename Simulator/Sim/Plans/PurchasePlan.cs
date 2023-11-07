using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class PurchasePlan
{
    private readonly PropertyValue _propertyValue;

    public PurchasePlan(
        PropertyValue propertyValue,
        decimal deposit,
        int mortgageTermYears,
        InterestRate mortgageInterestRate)
    {
        _propertyValue = propertyValue;
        Mortgage = new FixedMortgage(propertyValue.InitialValue - deposit, mortgageTermYears, mortgageInterestRate);
    }

    public FixedMortgage Mortgage { get; }

    public void ProcessMonth(out decimal mortgagePayment)
    {
        mortgagePayment = Mortgage.TakeMonthlyPayment();
    }

    public decimal ComputeEquity(Time time)
    {
        var liabilities = Mortgage.OutstandingLoan;
        var assets = _propertyValue.ComputeValue(time);
        return assets - liabilities;
    }
}
