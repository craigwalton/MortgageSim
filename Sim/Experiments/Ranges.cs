namespace PropertySim.Experiments;

public static class Ranges
{
    public static Range MortgageInterestRateRange { get; } = new(-0m, 0.2m, 0.01m);

    public static Range PropertyValueYearlyIncrease { get; } = new(-0.15m, 0.15m, 0.01m);

    public static Range InitialMonthlyRentPrice { get; } = new(500m, 2000m, 100m);

    public static Range SavingsInterestRateRange { get; } = new(-0.1m, 0.2m, 0.01m);
}
