using PropertySim.Variables;

namespace PropertySim.Accounts;

public class FixedMortgage
{
    private int _outstandingPayments;
    private readonly InterestRate _interestRate;
    private readonly decimal _monthlyPayment;
    private readonly StreamWriter _output;

    public FixedMortgage(decimal initialLoan, int initialTermYears, InterestRate interestRate, StreamWriter output)
    {
        OutstandingLoan = initialLoan;
        _outstandingPayments = 12 * initialTermYears;
        _interestRate = interestRate;
        _monthlyPayment = ComputeMonthlyPayment(initialLoan, interestRate, _outstandingPayments);
        _output = output;
    }

    public decimal OutstandingLoan { get; private set; }

    public decimal TakePayment()
    {
        var interest = OutstandingLoan * _interestRate.Monthly;
        var principal = _monthlyPayment - interest;
        OutstandingLoan -= principal;
        _outstandingPayments--;
        _output.WriteLine($"Mortgage payment={_monthlyPayment:C} (interest={interest:C}; principal={principal:C});" +
                          $"Loan={OutstandingLoan:C}");
        // TODO: ensure that final payment results in precisely outstanding loan of precisely 0.
        return _monthlyPayment;
    }

    private static decimal ComputeMonthlyPayment(decimal loan, InterestRate interest, int totalPayments)
    {
        if (interest.Monthly == 0m)
        {
            return loan / totalPayments;
        }
        var rateToPowerOfPayments = (1 + interest.Monthly).RaiseToPowerOf(totalPayments);
        return loan * (interest.Monthly * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
    }
}
