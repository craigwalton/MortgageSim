namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        var initialPropertyValue = new decimal(200_000);
        var deposit = new decimal(50_000);
        var mortgageInterestRate = InterestRate.FromYearlyPercentage(2.09m);
        var savingsInterestRate = InterestRate.FromYearlyPercentage(1m);
        var mortgage = new VariableMortgage(initialPropertyValue - deposit, 25);
        var savings = new Savings();
        var income = 1000m;

        for (var y = 0; y < mortgage.InitialTermYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                Console.WriteLine($"M{m:00}/Y{y:00}");
                var mortgagePayment = mortgage.MakePayment(mortgageInterestRate);
                var incomeForSavings = income - mortgagePayment;
                savings.MakePayment(incomeForSavings, savingsInterestRate);
            }
        }
        Console.WriteLine($"Final mortgage loan={mortgage.RemainingLoan:C}; Final savings={savings.Balance:C}");
    }
}