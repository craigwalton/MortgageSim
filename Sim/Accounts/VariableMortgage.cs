using PropertySim.Variables;

namespace PropertySim.Accounts;

public class VariableMortgage
{
    private readonly StreamWriter _output;
    private readonly InterestRate _interestRate;

    public VariableMortgage(
        decimal initialLoan,
        int initialTermYears,
        InterestRate interestRate,
        StreamWriter output)
    {
        InitialLoan = initialLoan;
        InitialTermYears = initialTermYears;
        _interestRate = interestRate;
        RemainingLoan = initialLoan;
        RemainingPayments = 12 * initialTermYears;
        _output = output;
    }

    public decimal InitialLoan { get; }

    public int InitialTermYears { get; }

    public decimal RemainingLoan { get; private set; }

    public int RemainingPayments { get; private set; }

    public decimal MakePayment()
    {
        var payment = ComputeMonthlyPayment();
        var interest = RemainingLoan * _interestRate.Monthly;
        var principal = payment - interest;
        RemainingLoan -= principal;
        RemainingPayments--;
        _output.WriteLine($"Mortgage payment={payment:C} (interest={interest:C}; principal={principal:C}); Loan={RemainingLoan:C}");
        return payment;
    }

    private decimal ComputeMonthlyPayment()
    {
        var interest = (double)_interestRate.Monthly;
        var rateToPowerOfPayments = Math.Pow(1 + interest, RemainingPayments);
        var result = (double)RemainingLoan * (interest * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
        return new decimal(Math.Round(result, 2));
    }
}
