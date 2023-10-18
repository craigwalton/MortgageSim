namespace PropertySim;

public readonly record struct InterestRate(decimal Value)
{
    public static InterestRate FromYearlyPercentage(decimal percentage)
    {
        return new InterestRate(percentage / 100m);
    }

    public decimal Yearly => Value;

    public decimal Monthly => Value / 12;
}