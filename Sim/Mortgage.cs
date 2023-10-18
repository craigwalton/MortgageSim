namespace PropertySim;

public sealed class Mortgage
{
    public Mortgage(decimal loan, InterestRate interestRate, int termYears)
    {
        Loan = loan;
        InterestRate = interestRate;
        TermYears = termYears;
        MonthlyPayment = ComputeMonthlyPayment();
    }

    public decimal Loan { get; }

    public InterestRate InterestRate { get; }

    public int TermYears { get; }

    public decimal MonthlyPayment { get; }

    public (decimal Principal, decimal Interest) ComputePayment(decimal balance)
    {
        var interest = balance * InterestRate.Monthly;
        var principal = MonthlyPayment - interest;
        return (principal, interest);
    }

    private decimal ComputeMonthlyPayment()
    {
        var numberOfPayments = TermYears * 12;
        var rate = (double)InterestRate.Monthly;
        var rateToPowerOfPayments = Math.Pow(1 + rate, numberOfPayments);
        var result = (double)Loan * (rate * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
        return new decimal(Math.Round(result, 2));
    }
}