namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        var initialPropertyValue = new decimal(200_000);
        var deposit = new decimal(50_000);
        var interestRate = InterestRate.FromYearlyPercentage(2.09m);
        var mortgage = new VariableMortgage(initialPropertyValue - deposit, 25);

        var balance = initialPropertyValue - deposit;

        for (var y = 0; y < mortgage.TermYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                var payment = mortgage.ComputePayment(balance, interestRate, mortgage.TermYears * 12 - (y * 12 + m));
                balance -= payment.Principal;
                Console.WriteLine(
                    $"M{m:00}/Y{y:00} payment: {payment.Principal + payment.Interest} " +
                    $"(interest={payment.Interest}; principal={payment.Principal})");
            }
        }

        Console.WriteLine($"Final mortgage balance: {balance}");
    }
}