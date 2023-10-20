namespace PropertySim;

public class Savings
{
    public decimal Balance { get; private set; }

    public void MakePayment(decimal payment, InterestRate interestRate)
    {
        var accruedInterest = Balance * interestRate.Monthly;
        Balance += accruedInterest + payment;
        Console.WriteLine($"Savings={Balance:C}");
    }
}