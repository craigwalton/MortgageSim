using PropertySim.Variables;

namespace PropertySim;

public static class Baseline
{
    public const decimal Deposit = 500;
    public const int MortgageTermYears = 25;

    public static readonly InterestRate MortgageInterestRate = new(0.02m);
    public static readonly InterestRate SavingsInterestRate = new(0.02m);
    public static readonly RentPrice RentPrice = new(500m, 0.02m);
    public static readonly PropertyValue PropertyValue = new(200_000m, 0.03m);
}
