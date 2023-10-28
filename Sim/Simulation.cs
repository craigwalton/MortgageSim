using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

public sealed class Simulation
{
    public Result Run(
        decimal deposit = 50_000m,
        int mortgageTermYears = 25,
        int simulationYears = 25,
        InterestRate? mortgageInterestRate = null,
        InterestRate? savingsInterestRate = null,
        RentPrice? rent = null,
        PropertyValue? propertyValue = null,
        StreamWriter? output = null)
    {
        // TODO: consider modelling these rates as offsets from the base rate.
        mortgageInterestRate ??= new InterestRate(0.0209m);
        savingsInterestRate ??= new InterestRate(0.001m);
        rent ??= new RentPrice(500m, 0.02m);
        propertyValue ??= new PropertyValue(200_000m, 0.03m);
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
            mortgageInterestRate.ProcessYearlyUpdate();
            savingsInterestRate.ProcessYearlyUpdate();
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
