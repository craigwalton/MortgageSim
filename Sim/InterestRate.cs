using MathNet.Numerics.Distributions;

namespace PropertySim;

public sealed class InterestRate
{
    private readonly IContinuousDistribution _yearlyInterest;

    public InterestRate(IContinuousDistribution yearlyInterest)
    {
        _yearlyInterest = yearlyInterest;
        ProcessYearlyUpdate();
    }

    public decimal Monthly { get; private set; }

    public void ProcessYearlyUpdate()
    {
        Monthly = (decimal)_yearlyInterest.Sample() / 12;
    }
}