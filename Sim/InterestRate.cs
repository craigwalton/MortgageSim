namespace PropertySim;

public readonly record struct InterestRate(double Value)
{
    public static InterestRate FromYearlyPercentage(double percentage)
    {
        return new InterestRate(percentage / 100d);
    }

    public double Yearly => Value;

    public double Monthly => Value / 12;

    public decimal MonthlyDecimal => new(Monthly);
}