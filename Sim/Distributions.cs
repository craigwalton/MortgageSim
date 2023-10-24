using MathNet.Numerics.Distributions;

namespace PropertySim;

public static class Distributions
{
    public static Normal Constant(double value)
    {
        return new Normal(value, 0.0);
    }
}
