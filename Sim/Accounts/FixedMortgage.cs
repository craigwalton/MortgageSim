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

    public decimal TakeMonthlyPayment()
    {
        return _outstandingPayments switch
        {
            < 1 => throw new InvalidOperationException("There are no outstanding payments."),
            > 1 => ProcessPayment(),
            1 => ProcessFinalPayment(),
        };
    }

    private decimal ProcessPayment()
    {
        var interest = OutstandingLoan * _interestRate.Monthly;
        var principal = _monthlyPayment - interest;
        OutstandingLoan -= principal;
        _outstandingPayments--;
        _output.WriteLine($"Mortgage payment={_monthlyPayment:C} (interest={interest:C}; principal={principal:C}); " +
                          $"Loan={OutstandingLoan:C}");
        return _monthlyPayment;
    }

    private decimal ProcessFinalPayment()
    {
        var interest = OutstandingLoan * _interestRate.Monthly;
        var principal = OutstandingLoan;
        var payment = interest + principal;
        OutstandingLoan = 0;
        _outstandingPayments = 0;
        _output.WriteLine($"Final mortgage payment={payment:C} (interest={interest:C}; principal={principal:C}); " +
                          $"Loan={OutstandingLoan:C}");
        return payment;
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
