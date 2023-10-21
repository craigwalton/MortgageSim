using MathNet.Numerics.Distributions;
using PropertySim.Plans;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        const decimal initialPropertyValue = 200_000m;
        const decimal deposit = 50_000m;
        const int mortgageTermYears = 25;
        const decimal income = 1000m;
        var mortgageInterestRate = InterestRate.FromYearlyPercentage(2.09m);
        var savingsInterestRate = InterestRate.FromYearlyPercentage(1m);
        var rent = new RentPrice(500m, new Normal(1.02, 0.02));

        var output = new StreamWriter(Console.OpenStandardOutput());
        output.AutoFlush = true;
        // output = StreamWriter.Null;

        var purchasePlan = new HousePurchasePlan(initialPropertyValue, deposit, mortgageTermYears, output);
        var rentalPlan = new HouseRentalPlan(deposit, rent, output);

        for (var y = 0; y < mortgageTermYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                output.WriteLine($"M{m:00}/Y{y:00}");
                purchasePlan.ProcessMonth(income, mortgageInterestRate, savingsInterestRate);
                rentalPlan.ProcessMonth(income, mortgageInterestRate, savingsInterestRate);
            }
            rent.ProcessYearlyUpdate();
        }

        output.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity():C}");
        output.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity():C}");
    }
}