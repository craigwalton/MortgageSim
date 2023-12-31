using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using MortgageSim.Sim.Variables;

namespace MortgageSim.Sim.Accounts;

internal sealed class FixedMortgage
{
    private readonly InterestRate _interestRate;
    private readonly decimal _monthlyPayment;
    private int _outstandingPayments;

    public FixedMortgage(decimal initialLoan, int initialTermYears, InterestRate interestRate)
    {
        Guard.IsGreaterThan(initialLoan, 0m);
        Guard.IsGreaterThan(initialTermYears, 0m);

        OutstandingLoan = initialLoan;
        _outstandingPayments = 12 * initialTermYears;
        _interestRate = interestRate;
        _monthlyPayment = ComputeMonthlyPayment(initialLoan, interestRate, _outstandingPayments);
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
        Debug.WriteLine($"Mortgage payment={_monthlyPayment:C} (interest={interest:C}; principal={principal:C}); " +
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
        Debug.WriteLine($"Final mortgage payment={payment:C} (interest={interest:C}; principal={principal:C}); " +
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
