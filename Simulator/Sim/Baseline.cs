using PropertySim.Variables;

namespace PropertySim;

/// <threadsafety static="true" instance="true"/>
public static class Baseline
{
    public const int SimulationDurationYears = 5;
    public const decimal Deposit = 30_000m;
    public static readonly PropertyValue PropertyValue = new(200_000m, 0.025m);
    public const int MortgageTermYears = 25;
    public static readonly InterestRate MortgageInterestRate = new(0.05m);
    public static readonly RentPrice RentPrice = new(1200m, 0.03m);
    public static readonly InterestRate SavingsInterestRate = new(0.05m);
}
