namespace PropertySim;

public class VariableMortgage
{
    public VariableMortgage(decimal loan, int termYears)
    {
        Loan = loan;
        TermYears = termYears;
    }

    public decimal Loan { get; }

    public int TermYears { get; }

    public (decimal Principal, decimal Interest) ComputePayment(decimal balance, InterestRate rate, int monthsRemaining)
    {
        var interest = balance * rate.Monthly;
        var principal = ComputeMonthlyPayment(balance, rate, monthsRemaining) - interest;
        return (principal, interest);
    }
    
    private static decimal ComputeMonthlyPayment(decimal balance, InterestRate rate, int monthsRemaining)
    {
        var numberOfPayments = monthsRemaining;
        var interest = (double)rate.Monthly;
        var rateToPowerOfPayments = Math.Pow(1 + interest, numberOfPayments);
        var result = (double)balance * (interest * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
        return new decimal(Math.Round(result, 2));
    }
}