namespace PropertySim.Variables;

public sealed class InterestRate
{
    public InterestRate(decimal yearly)
    {
        Yearly = yearly;
        Monthly = yearly / 12;
    }

    public decimal Yearly { get; }

    public decimal Monthly { get; private set; }
}