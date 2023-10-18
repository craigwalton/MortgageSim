namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        var initialPropertyValue = new decimal(200_000);
        var deposit = new decimal(50_000);
        var interestRate = InterestRate.FromYearlyPercentage(2.09);
        const int mortgageTermYears = 25;
        var initialLoanValue = initialPropertyValue - deposit;
        var monthlyPayment = ComputeMonthlyPayment();

        var balance = initialPropertyValue - deposit;

        for (var y = 0; y < mortgageTermYears; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                var interest = balance * interestRate.MonthlyDecimal;
                var principal = monthlyPayment - interest;
                balance -= principal;
                Console.WriteLine(
                    $"M{m:00}/Y{y:00} payment: {monthlyPayment} (interest={interest}; principal={principal})");
            }
        }

        Console.WriteLine($"Final mortgage balance: {balance}");

        decimal ComputeMonthlyPayment()
        {
            var numberOfPayments = mortgageTermYears * 12;
            var rate = interestRate.Monthly;
            var rateToPowerOfPayments = Math.Pow(1 + rate, numberOfPayments);
            var result = (double)initialLoanValue * (rate * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
            return new decimal(Math.Round(result, 2));
        }
    }
}
