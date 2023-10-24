using PropertySim.Variables;

namespace PropertySim.Accounts;

public class Savings
{
    private readonly StreamWriter _output;

    public Savings(decimal initialBalance, StreamWriter output)
    {
        Balance = initialBalance;
        _output = output;
    }

    public decimal Balance { get; private set; }

    public void MakePayment(decimal payment, InterestRate interestRate)
    {
        var accruedInterest = Balance * interestRate.Monthly;
        Balance += accruedInterest + payment;
        _output.WriteLine($"Savings payment={payment:C}; Interest={accruedInterest:C}; Balance={Balance:C}");
    }
}