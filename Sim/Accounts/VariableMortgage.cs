using PropertySim.Variables;

namespace PropertySim.Accounts;

// TODO: This can be a FixedMortgage now.
public class VariableMortgage
{
    private int _outstandingPayments;
    private readonly InterestRate _interestRate;
    private readonly StreamWriter _output;

    public VariableMortgage(
        decimal initialLoan,
        int initialTermYears,
        InterestRate interestRate,
        StreamWriter output)
    {
        OutstandingLoan = initialLoan;
        _outstandingPayments = 12 * initialTermYears;
        _interestRate = interestRate;
        _output = output;
    }

    public decimal OutstandingLoan { get; private set; }

    public decimal TakePayment()
    {
        var payment = ComputeMonthlyPayment();
        var interest = OutstandingLoan * _interestRate.Monthly;
        var principal = payment - interest;
        OutstandingLoan -= principal;
        _outstandingPayments--;
        _output.WriteLine($"Mortgage payment={payment:C} (interest={interest:C}; principal={principal:C}); Loan={OutstandingLoan:C}");
        return payment;
    }

    private decimal ComputeMonthlyPayment()
    {
        if (_interestRate.Monthly == 0m)
        {
            return OutstandingLoan / _outstandingPayments;
        }
        var interest = (double)_interestRate.Monthly;
        var rateToPowerOfPayments = Math.Pow(1 + interest, _outstandingPayments);
        var result = (double)OutstandingLoan * (interest * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
        return new decimal(Math.Round(result, 2));
    }
}
