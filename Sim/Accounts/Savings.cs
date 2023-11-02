using PropertySim.Variables;

namespace PropertySim.Accounts;

public class Savings
{
    private readonly InterestRate _interestRate;
    private readonly StreamWriter _output;

    public Savings(decimal initialBalance, InterestRate interestRate, StreamWriter output)
    {
        Balance = initialBalance;
        _interestRate = interestRate;
        _output = output;
    }

    public decimal Balance { get; private set; }

    public void MakeMonthlyPayment(decimal payment)
    {
        // TODO: Should we allow negative payments? Perhaps only if it doesn't result in the balance being negative?
        var accruedInterest = Balance * _interestRate.Monthly;
        Balance += accruedInterest + payment;
        // if (Balance < 0m)
        // {
        //     throw new InvalidOperationException(
        //         $"A payment of {payment:C} will result in this account being overdrawn: {Balance:C}.");
        // }
        _output.WriteLine($"Savings payment={payment:C}; Interest={accruedInterest:C}; Balance={Balance:C}");
    }
}