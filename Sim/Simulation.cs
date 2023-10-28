using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

public sealed class Simulation
{
    public Result Run(
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
            propertyValue,
            deposit,
            mortgageTermYears,
            mortgageInterestRate,
            output);
        var rentalPlan = new HouseRentalPlan(deposit, rent, savingsInterestRate, output);

        for (var y = 0; y < simulationYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                output.WriteLine($"## M{m:00}/Y{y:00} ##");
                purchasePlan.ProcessMonth(out var payment);
                rentalPlan.ProcessMonth(payment);
            }
            propertyValue.ProcessYearlyUpdate();
            rent.ProcessYearlyUpdate();
        }

        output.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity():C}");
        output.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity():C}");

        return new Result(purchasePlan.ComputeEquity(), rentalPlan.ComputeEquity());
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
