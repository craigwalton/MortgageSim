using System.Diagnostics;
using PropertySim.Variables;

namespace PropertySim.Accounts;

internal sealed class Savings
{
    private readonly InterestRate _interestRate;

    public Savings(decimal initialBalance, InterestRate interestRate)
    {
        Balance = initialBalance;
        _interestRate = interestRate;
    }

    public decimal Balance { get; private set; }

    public void MakeMonthlyPayment(decimal payment)
    {
        // Note: negative payments (withdrawals) are permitted, even if the account becomes overdrawn.
        // Typically, a savings account would forbid overdrafts or would charge a higher interest rate on the overdraft.
        // However, given that the goal is to simply compute the difference in net worth between buying and renting
        // without any consideration of affordability, we assume that any additional funds required in the rent
        // scenario (the overdraft) would have been deposited in an equivalent savings account in the mortgage scenario.
        var accruedInterest = Balance * _interestRate.Monthly;
        Balance += accruedInterest + payment;
        Debug.WriteLine($"Savings payment={payment:C}; Interest={accruedInterest:C}; Balance={Balance:C}");
    }
}