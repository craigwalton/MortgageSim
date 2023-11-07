using PropertySim.Accounts;
using PropertySim.Variables;

namespace PropertySim.Plans;

public sealed class PurchasePlan
{
    private readonly PropertyValue _propertyValue;
    private readonly FixedMortgage _mortgage;

    public PurchasePlan(
        PropertyValue propertyValue,
        decimal deposit,
        int mortgageTermYears,
        InterestRate mortgageInterestRate)
    {
        _propertyValue = propertyValue;
        _mortgage = new FixedMortgage(propertyValue.InitialValue - deposit, mortgageTermYears, mortgageInterestRate);
    }

    public void ProcessMonth(out decimal mortgagePayment)
    {
        mortgagePayment = _mortgage.TakeMonthlyPayment();
    }

    public decimal ComputeEquity(Time time)
    {
        var liabilities = _mortgage.OutstandingLoan;
        var assets = _propertyValue.ComputeValue(time);
        return assets - liabilities;
    }
}
