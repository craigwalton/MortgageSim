namespace PropertySim;

public class VariableMortgage
{
    public VariableMortgage(decimal initialLoan, int initialTermYears)
    {
        InitialLoan = initialLoan;
        InitialTermYears = initialTermYears;
        RemainingLoan = initialLoan;
        RemainingPayments = 12 * initialTermYears;
    }

    public decimal InitialLoan { get; }

    public int InitialTermYears { get; }

    public decimal RemainingLoan { get; private set; }

    public int RemainingPayments { get; private set; }

    public decimal MakePayment(InterestRate rate)
    {
        var payment = ComputeMonthlyPayment(rate);
        var interest = RemainingLoan * rate.Monthly;
        var principal = payment - interest;
        RemainingLoan -= principal;
        RemainingPayments--;
        Console.WriteLine($"payment={payment:C} (interest={interest:C}; principal={principal:C}); loan={RemainingLoan:C}");
        return payment;
    }

    private decimal ComputeMonthlyPayment(InterestRate rate)
    {
        var interest = (double)rate.Monthly;
        var rateToPowerOfPayments = Math.Pow(1 + interest, RemainingPayments);
        var result = (double)RemainingLoan * (interest * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
        return new decimal(Math.Round(result, 2));
    }
}
