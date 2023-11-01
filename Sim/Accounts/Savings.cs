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
        var accruedInterest = Balance * _interestRate.Monthly;
        Balance += accruedInterest + payment;
        _output.WriteLine($"Savings payment={payment:C}; Interest={accruedInterest:C}; Balance={Balance:C}");
    }
}