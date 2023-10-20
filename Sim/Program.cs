using PropertySim.Plans;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        const decimal initialPropertyValue = 200_000m;
        const decimal deposit = 50_000m;
        const int mortgageTermYears = 25;
        var mortgageInterestRate = InterestRate.FromYearlyPercentage(2.09m);
        var savingsInterestRate = InterestRate.FromYearlyPercentage(1m);
        const decimal income = 1000m;
        const decimal monthlyRent = 500m;

        var purchasePlan = new HousePurchasePlan(initialPropertyValue, deposit, mortgageTermYears);
        var rentalPlan = new HouseRentalPlan(deposit, monthlyRent);

        for (var y = 0; y < mortgageTermYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                Console.WriteLine($"M{m:00}/Y{y:00}");
                purchasePlan.ProcessMonth(income, mortgageInterestRate, savingsInterestRate);
                rentalPlan.ProcessMonth(income, mortgageInterestRate, savingsInterestRate);
            }
        }

        Console.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity():C}");
        Console.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity():C}");
    }
}