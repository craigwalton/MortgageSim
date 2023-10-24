using MathNet.Numerics.Distributions;

namespace PropertySim.Variables;

public sealed class RentPrice
{
    private readonly IContinuousDistribution _yearlyIncrease;

    public RentPrice(decimal initialMonthly, IContinuousDistribution yearlyIncrease)
    {
        MonthlyPrice = initialMonthly;
        _yearlyIncrease = yearlyIncrease;
    }

    public decimal MonthlyPrice { get; private set; }

    public void ProcessYearlyUpdate()
    {
        MonthlyPrice *= 1 + (decimal)_yearlyIncrease.Sample();
    }
}
