using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

public sealed class Simulation
{
    public static Result Run(
        decimal deposit = Baseline.Deposit,
        int mortgageTermYears = Baseline.MortgageTermYears,
        int simulationYears = 25,
        InterestRate? mortgageInterestRate = null,
        InterestRate? savingsInterestRate = null,
        RentPrice? rent = null,
        PropertyValue? propertyValue = null,
        StreamWriter? output = null)
    {
        mortgageInterestRate ??= Baseline.MortgageInterestRate;
        savingsInterestRate ??= Baseline.SavingsInterestRate;
        rent ??= Baseline.RentPrice;
        propertyValue ??= Baseline.PropertyValue;
        output ??= StreamWriter.Null;

        var purchasePlan = new HousePurchasePlan(
            propertyValue.Value,
            deposit,
            mortgageTermYears,
            mortgageInterestRate.Value,
            output);
        var rentalPlan = new HouseRentalPlan(deposit, rent.Value, savingsInterestRate.Value, output);

        Time time;
        for (time = new Time(); time.Year < simulationYears; time.AdvanceOneMonth())
        {
            output.WriteLine($"## {time} ##");
            purchasePlan.ProcessMonth(out var payment);
            rentalPlan.ProcessMonth(payment, time);
        }

        output.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity(time):C}");
        output.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity(time):C}");

        return new Result(purchasePlan.ComputeEquity(time), rentalPlan.ComputeEquity(time));
    }

    public readonly record struct Result(decimal PurchaseEquity, decimal RentEquity)
    {
        public override string ToString()
        {
            return $"Purchase equity={PurchaseEquity:C}; Rent equity={RentEquity:C}";
        }

        public decimal ComputeDelta()
        {
            return PurchaseEquity - RentEquity;
        }
    }
}
