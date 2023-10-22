using MathNet.Numerics.Distributions;
using PropertySim.Plans;

namespace PropertySim;

public sealed class Simulation
{
    public Result Run(
        decimal deposit = 50_000m,
        // TODO: can this be removed and instead assume only the amount which went towards mortgage is saved?
        decimal income = 1000m,
        int mortgageTermYears = 25,
        int simulationYears = 25,
        InterestRate? mortgageInterestRate = null,
        InterestRate? savingsInterestRate = null,
        RentPrice? rent = null,
        PropertyValue? propertyValue = null,
        StreamWriter? output = null)
    {
        // TODO: consider modelling these rates as offsets from the base rate.
        mortgageInterestRate ??= new InterestRate(new Normal(0.0209, 0.01));
        savingsInterestRate ??= new InterestRate(new Normal(0.001, 0.01));
        rent ??= new RentPrice(500m, new Normal(0.02, 0.02));
        propertyValue ??= new PropertyValue(200_000m, new Normal(0.03, 0.01));
        output ??= StreamWriter.Null;

        var purchasePlan = new HousePurchasePlan(
            propertyValue,
            deposit,
            mortgageTermYears,
            mortgageInterestRate,
            savingsInterestRate,
            output);
        var rentalPlan = new HouseRentalPlan(deposit, rent, savingsInterestRate, output);

        for (var y = 0; y < simulationYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                output.WriteLine($"M{m:00}/Y{y:00}");
                purchasePlan.ProcessMonth(income);
                rentalPlan.ProcessMonth(income);
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
    }
}
