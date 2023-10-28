namespace PropertySim.Variables;

public sealed class InterestRate
{
    public InterestRate(decimal yearlyInterest)
    {
        Monthly = yearlyInterest / 12;
    }

    public decimal Monthly { get; private set; }
}