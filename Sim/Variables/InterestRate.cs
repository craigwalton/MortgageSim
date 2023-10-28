namespace PropertySim.Variables;

public readonly record struct InterestRate(decimal Yearly, decimal Monthly)
{
    public InterestRate(decimal yearly)
        : this(yearly, yearly / 12)
    {
    }
}