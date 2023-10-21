using MathNet.Numerics.Distributions;

namespace PropertySim;

public class PropertyValue
{
    private readonly IContinuousDistribution _yearlyIncrease;

    public PropertyValue(decimal initialValue, IContinuousDistribution yearlyIncrease)
    {
        Value = initialValue;
        _yearlyIncrease = yearlyIncrease;
    }

    public decimal Value { get; private set; }

    public void ProcessYearlyUpdate()
    {
        Value *= 1 + (decimal)_yearlyIncrease.Sample();
    }
}
